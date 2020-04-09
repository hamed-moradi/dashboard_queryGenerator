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
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Domain.Application.Services
{
    public class Match2ClubService : BaseService<Match2Club>, IMatch2ClubService
    {
        private readonly IRepository<Match2Club> _repository;
        private readonly IRepository<ChangeLog> _logRepository;

        public Match2ClubService(IRepository<Match2Club> repository) : base(repository)
        {
            _repository = repository;
            _logRepository = ServiceLocatorAdapter.Current.GetInstance<IRepository<ChangeLog>>();
        }

        public int SetScore(Match2Club model, int adminId, string data = "")
        {
            ValidateModelByAttribute(model, typeof(UpdateMandatoryField));
            var parameters = new DynamicParameters();
            parameters.Add("@MatchId", model.MatchId, DbType.Int32);
            parameters.Add("@AwayClubId", model.AwayClubId, DbType.Int32);
            parameters.Add("@AwayClubScore", model.AwayClubScore, DbType.Int32);
            parameters.Add("@HomeClubId", model.HomeClubId, DbType.Int32);
            parameters.Add("@HomeClubScore", model.HomeClubScore, DbType.Int32);
            parameters.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Result", dbType: DbType.Boolean, direction: ParameterDirection.ReturnValue);
            _repository.QueryFirstOrDefault("[dbo].[Panel_Match_SetResult]", parameters, commandType: CommandType.StoredProcedure);

            var statusCode = parameters.Get<int>("@StatusCode");

            // Add Log
            if (statusCode > 0)
            {
                var modelId = (int)model.GetType().GetProperties().SingleOrDefault(prop => prop.Name.Equals(nameof(Entity.Id))).GetValue(model, null);
                _logRepository.Insert(new ChangeLog { Entity = typeof(Admin).Name, EntityId = modelId, ActionType = Convert.ToByte(ActionType.Update), AdminId = adminId, Data = data, CreatedAt = DateTime.Now });
            }

            return statusCode;
        }
    }
}
