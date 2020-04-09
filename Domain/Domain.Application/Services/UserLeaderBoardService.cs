using Dapper;
using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model._App;
using Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Domain.Application.Services
{
    public class UserLeaderBoardService : BaseService<UserLeaderBoard>, IUserLeaderBoardService
    {
        #region Constructor
        private readonly IRepository<UserLeaderBoard> _repository;
        public UserLeaderBoardService(IRepository<UserLeaderBoard> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion

        public IList<UserLeaderBoard> GetTotalPoints(UserLeaderBoard model, out long totalCount, string orderBy = "TotalPoints", string order = "Desc", int skip = 0, int take = 10)
        {
            ValidateModelByAttribute(model, typeof(UpdateMandatoryField));

            var parameters = new DynamicParameters();
            parameters.Add("@EventId", model.EventId, DbType.Int32);
            parameters.Add("@UserId", model.Id, DbType.Int32);
            parameters.Add("@UserName", model.UserName, DbType.String);
            parameters.Add("@OrderBy", orderBy, DbType.String);
            parameters.Add("@Order", order, DbType.String);
            parameters.Add("@Skip", skip, DbType.Int32);
            parameters.Add("@Take", take, DbType.Int32);

            var response = _repository.Query("Panel_User_GetLeaderboardPaging", parameters, commandType: CommandType.StoredProcedure);
            totalCount = response.Any() ? response.First().RowsCount : 0;
            return response.ToList();
        }
    }
}