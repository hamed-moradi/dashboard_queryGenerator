using Asset.Infrastructure._App;
using Dapper;
using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model;
using Domain.Model._App;
using Domain.Model.Entities;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Domain.Application.Services
{
    public class MatchService : BaseService<Match>, IMatchService
    {
        IRepository<Match> _repository;
        IRepository<Match2Club> _match2ClubRepository;
        private readonly IRepository<ChangeLog> _logRepository;

        public MatchService(IRepository<Match> repository) : base(repository)
        {
            _repository = repository;
            _logRepository = ServiceLocatorAdapter.Current.GetInstance<IRepository<ChangeLog>>();
            _match2ClubRepository = ServiceLocatorAdapter.Current.GetInstance<IRepository<Match2Club>>();
        }

        public bool CheckExist(Match model, int? id)
        {
            var query = $"SELECT m.* , m2c.AwayClubId , m2c.HomeClubId FROM dbo.Match m INNER JOIN dbo.Match2Club m2c ON m.Id = m2c.MatchId " +
            $"WHERE m2c.HomeClubId = @hId AND m2c.AwayClubId = @aId AND m.EventId = @eId AND m.Status = 1 ";

            var parameters = new DynamicParameters();
            parameters.Add("hId", model.HomeClubId);
            parameters.Add("aId", model.AwayClubId);
            parameters.Add("eId", model.EventId);

            var result = _repository.Query(query, parameters);
            return id.HasValue ? result.Any(x => x.Id != id) : result.Any();
        }

        public override int InsertWithLog(Match model, int adminId, string data = "")
        {
            int newMatchId;
            using (var transactionScope = new TransactionScope())
            {
                ValidateModelByAttribute(model, typeof(InsertMandatoryField));
                newMatchId = _repository.Insert(model);

                var newMatch2ClubId = _match2ClubRepository.Insert(
                        new Match2Club
                        {
                            AwayClubId = model.AwayClubId,
                            HomeClubId = model.HomeClubId,
                            MatchId = newMatchId
                        });
                _logRepository.Insert(new ChangeLog { Entity = typeof(Match).Name, EntityId = newMatchId, ActionType = Convert.ToByte(ActionType.Create), AdminId = adminId, Data = data, CreatedAt = DateTime.Now });

                transactionScope.Complete();
            }
            return newMatchId;
        }

        public override int UpdateWithLog(Match model, int adminId, string data = "")
        {
            int id;
            using (var transactionScope = new TransactionScope())
            {
                ValidateModelByAttribute(model, typeof(UpdateMandatoryField));
                id = _repository.Update(model);

                var match2ClubModel = _match2ClubRepository.Find(new Match2Club { MatchId = model.Id }).FirstOrDefault();
                if (match2ClubModel != null)
                {
                    match2ClubModel.HomeClubId = model.HomeClubId;
                    match2ClubModel.AwayClubId = model.AwayClubId;

                    var statusIdMatch2Club = _match2ClubRepository.Update(match2ClubModel);
                }

                if (id > 0)
                {
                    var modelId = (int)model.GetType().GetProperties().SingleOrDefault(prop => prop.Name.Equals(nameof(Entity.Id))).GetValue(model, null);
                    _logRepository.Insert(new ChangeLog { Entity = typeof(Match).Name, EntityId = modelId, ActionType = Convert.ToByte(ActionType.Update), AdminId = adminId, Data = data, CreatedAt = DateTime.Now });
                }

                transactionScope.Complete();
            }
            return id;
        }
    }
}
