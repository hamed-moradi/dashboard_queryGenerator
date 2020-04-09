using System.Collections.Generic;

using Domain.Model.Entities;
using Dapper;
using Domain.Application.BaseService;
using Domain.Application.Repository;
using System.Linq;
using Asset.Infrastructure.Common;
using System;

namespace Domain.Application.Services
{
    public class DropDownService : BaseService<DropDownItemModel>, IDropDownService
    {
        #region Constructor

        private readonly IRepository<DropDownItemModel> _repository;

        public DropDownService(IRepository<DropDownItemModel> repository) : base(repository)
        {
            _repository = repository;
        }

        #endregion Constructor

        public IEnumerable<DropDownItemModel> GetRoles()
        {
            var query = $"SELECT [{nameof(Role.Id)}],[{nameof(Role.Title)}] AS {nameof(DropDownItemModel.Name)} FROM [{nameof(Role)}] WITH(NOLOCK) " +
                $"WHERE [{nameof(Role.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
            return _repository.Query(query);
        }

        public IEnumerable<DropDownItemModel> GetEvents()
        {
            var query = $"SELECT [{nameof(Event.Id)}],[{nameof(Event.Title)}] AS {nameof(DropDownItemModel.Name)} FROM [{nameof(Event)}] WITH(NOLOCK) " +
                $"WHERE [{nameof(Event.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
            return _repository.Query(query);
        }

        public IEnumerable<DropDownItemModel> GetMatches(int eventId, string key = "")
        {
            var query = $"SELECT [{nameof(Match.Id)}],[{nameof(Match.Title)}] AS {nameof(DropDownItemModel.Name)} FROM [{nameof(Match)}] WITH(NOLOCK) " +
                $"WHERE [{nameof(Match.EventId)}] = {eventId} AND [{nameof(Match.Title)}] LIKE '%{key}%' AND [{nameof(Match.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
            return _repository.Query(query);
        }

        public IEnumerable<DropDownItemModel> GetClubs(int eventId, string key = "")
        {
            var query = $"SELECT [{nameof(Club.Id)}],[{nameof(Club.Name)}] AS {nameof(DropDownItemModel.Name)} FROM [{nameof(Club)}] WITH(NOLOCK) " +
                $"WHERE [{nameof(Club.EventId)}] = {eventId} AND [{nameof(Club.Name)}] LIKE '%{key}%' AND [{nameof(Club.Status)}]!={(byte)GeneralEnums.Status.Deleted}";
            return _repository.Query(query);
        }

        public IEnumerable<DropDownItemModel> GetEvents(string key, int skip, int take, out long totalCount)
        {
            var query = $"With Temp AS (SELECT Row_Number() Over(Order By [{nameof(Event.Id)}] Asc) AS RowNum,[{nameof(Event.Id)}]," +
                $"[{nameof(Event.Title)}] AS {nameof(DropDownItemModel.Name)} " +
                $"FROM [{nameof(Event)}] WITH(NOLOCK) WHERE [{nameof(Event.Title)}] LIKE @{nameof(key)} AND [{nameof(Event.Status)}]!={(byte)GeneralEnums.Status.Deleted}) " +
                $"SELECT * FROM Temp[{nameof(Event)}] WHERE RowNum > {skip} AND RowNum <= {take}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(key), $"%{key}%");
            var result = _repository.Query(query, parameters);
            totalCount = result.Count();
            return result;
        }

        public IEnumerable<DropDownItemModel> GetMatchGroupes(int eventId, string key = "")
        {
            var query = $"SELECT [{nameof(MatchGroup.Id)}],[{nameof(MatchGroup.Title)}] AS {nameof(DropDownItemModel.Name)} FROM [{nameof(MatchGroup)}] WITH(NOLOCK) " +
                $"WHERE [{nameof(MatchGroup.EventId)}] = {eventId} AND [{nameof(MatchGroup.Title)}] LIKE '%{key}%'";
            return _repository.Query(query);
        }
    }
}