using System.Collections.Generic;
using Domain.Application.BaseService;
using Domain.Application.Repository;
using Domain.Model.Entities;
using Dapper;
using System.Linq;
using System;

namespace Domain.Application.Services
{
    public class PredictionService : BaseService<Prediction>, IPredictionService
    {
        private readonly IRepository<Prediction> _repository;
        public PredictionService(IRepository<Prediction> repository) : base(repository)
        {
            _repository = repository;
        }

        public override IEnumerable<Prediction> GetPaging(Prediction model, out long totalCount, string orderBy, string order, int skip, int take)
        {
            var query = $"SELECT COUNT(1) OVER() AS RowsCount, [Prediction].* ,[User].[UserName] AS UserName,[Match].[EventId] AS EventId,[Match].[Title] AS MatchTitle,[Match].[Image] AS MatchImage,[Club].[Name] AS HomeClubName,[Club16].[Name] AS AwayClubName,[UserMatchPoint17].[Point] AS UserPoint FROM [Prediction] " +
                        "INNER JOIN[User] AS[User] WITH(NOLOCK) ON[Prediction].[UserId] = [User].[Id]" +
                        "INNER JOIN[Match] AS[Match] WITH(NOLOCK) ON[Prediction].[MatchId] = [Match].[Id]" +
                        //"INNER JOIN[Match] AS[Match14] WITH(NOLOCK) ON[Prediction].[MatchId] = [Match14].[Id]" +
                        "INNER JOIN[Club] AS[Club] WITH(NOLOCK) ON[Prediction].[HomeClubId] = [Club].[Id]" +
                        "INNER JOIN[Club] AS[Club16] WITH(NOLOCK) ON[Prediction].[AwayClubId] = [Club16].[Id]" +
                        "LEFT JOIN[UserMatchPoint] AS[UserMatchPoint17] WITH(NOLOCK) ON[Prediction].[Id] = [UserMatchPoint17].[PredictionId] WHERE 1=1";

            if (model.UserId.HasValue)
            {
                query += $"AND [User].[Id] = '{model.UserId.Value}' ";
            }

            if (model.UserPoint.HasValue)
            {
                query += $"AND [UserMatchPoint17].[Point] = {model.UserPoint.Value} ";
            }

            if (model.FromDate.HasValue)
            {
                query += $"AND [Prediction].[CreatedAt] >= '{model.FromDate.Value.ToShortDateString()}' ";
            }

            if (model.ToDate.HasValue)
            {
                query += $"AND [Prediction].[CreatedAt] <= '{model.ToDate.Value.AddDays(1).ToShortDateString()}' ";
            }

            if (model.EventId.HasValue && model.EventId != -1)
            {
                query += $"AND [Match].EventId = {model.EventId.Value} ";

                if (model.MatchId.HasValue && model.MatchId.Value != 0)
                {
                    query += $"AND [Match].Id = {model.MatchId.Value} ";
                }

                if (model.HomeClubId.HasValue && model.HomeClubId.Value != 0)
                {
                    query += $"AND ( HomeClubId = {model.HomeClubId.Value} OR AwayClubId = {model.HomeClubId.Value} ) ";
                }
            }         

            query += $" {GetOrderByFieldName(orderBy)} {order} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";


            var response = _repository.Query(query, null);
            totalCount = response.Any() ? response.First().RowsCount : 0;
            return response;
        }

        private string GetOrderByFieldName(string orderBy)
        {
            var result = "ORDER BY ";
            switch (orderBy)
            {
                case "Id": result += $"[{nameof(Prediction)}].[Id]"; break;
                case "UserName": result += $"[{nameof(User)}].[UserName]";break;
                case "MatchTitle": result += $"[{nameof(Match)}].[Title]"; break;
                case "HomeClubName": result += $"[{nameof(Club)}].[Name]"; break;
                case "HomeClubScore": result += $"[{nameof(Prediction)}].[HomeClubScore]"; break;
                case "AwayClubScore": result += $"[{nameof(Prediction)}].[AwayClubScore]"; break;
                case "AwayClubName": result += $"[Club16].[Name]"; break;
            }

            return result;
        }
    }
}
