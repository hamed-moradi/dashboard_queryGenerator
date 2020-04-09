using System.Collections.Generic;
using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model.Entities;
using Dapper;
using System.Linq;
using System;

namespace Domain.Application.Services
{
    public class PredictionStatisticsService : BaseService<PredictionStatistics>, IPredictionStatisticsService
    {
        private readonly IRepository<PredictionStatistics> _repository;

        public PredictionStatisticsService(IRepository<PredictionStatistics> repository) : base(repository)
        {
            _repository = repository;
        }

        public PredictionStatistics GetStatistics(Prediction model)
        {
            var parameters = new DynamicParameters();

            var where = "Where 1=1";

            if (model.UserId.HasValue)
            {
                where += $" AND [Prediction].UserId=@{nameof(Prediction.UserId)}";
                parameters.Add(nameof(Prediction.UserId), model.UserId.Value);
            }

            if (model.EventId.HasValue && model.EventId.Value != -1)
            {
                where += $" AND [Prediction].MatchId in (select {nameof(Match.Id)} from {nameof(Match)} where {nameof(Match.EventId)}=@{nameof(Match.EventId)})";
                parameters.Add(nameof(Match.EventId), model.EventId.Value);
            }

            if (model.MatchId.HasValue)
            {
                where += $" AND Prediction.MatchId=@{nameof(Prediction.MatchId)}";
                parameters.Add(nameof(Prediction.MatchId), model.MatchId.Value);
            }

            if (model.MatchId.HasValue)
            {
                where += $" AND Prediction.MatchId=@{nameof(Prediction.MatchId)}";
                parameters.Add(nameof(Prediction.MatchId), model.MatchId.Value);
            }

            if (model.HomeClubId.HasValue)
            {
                where += $" AND ( Prediction.HomeClubId = {model.HomeClubId.Value} OR Prediction.AwayClubId = {model.HomeClubId.Value} ) ";
            }

            if (model.UserPoint.HasValue)
            {
                where += $" AND [UserMatchPoint].Point=@{nameof(UserMatchPoint.Point)}";
                parameters.Add(nameof(UserMatchPoint.Point), model.UserPoint.Value);
            }

            if (model.FromDate.HasValue)
            {
                where += $" AND {nameof(Prediction.CreatedAt)} >= @FromDate";
                parameters.Add("@FromDate", model.FromDate.Value.ToShortDateString());
            }

            if (model.ToDate.HasValue)
            {
                where += $" AND {nameof(Prediction.CreatedAt)} <= @ToDate";
                parameters.Add("@ToDate", model.ToDate.Value.AddDays(1).ToShortDateString());
            }

            //var query = $"SELECT COUNT(1) OVER() AS RowsCount, ISNULL(Sum({nameof(UserMatchPoint.Point)}), 0) AS TotalPoints, (SELECT COUNT(1) FROM {nameof(Prediction)} {whereCount}) AS PredictionCount FROM {nameof(UserMatchPoint)} {wherePoint}";
            var query = $"select ISNULL(Sum(Point), 0) AS TotalPoints , Count(Prediction.Id) AS PredictionCount  from {nameof(Prediction)} left join {nameof(UserMatchPoint)} on [UserMatchPoint].PredictionId = [Prediction].Id {where}";

            var result = _repository.QueryFirstOrDefault(query, parameters);
            return result;
        }
    }
}