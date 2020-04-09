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
    public class UserService : BaseService<User>, IUserService
    {
        #region Constructor
        private readonly IRepository<User> _repository;
        private readonly IRepository<SentReceivedSMS> _smsRepository;
        private readonly ISessionService _sessionService;
        public UserService(IRepository<User> repository, ISessionService sessionService, IRepository<SentReceivedSMS> smsRepository) : base(repository)
        {
            _repository = repository;
            _sessionService = sessionService;
            _smsRepository = smsRepository;
        }

        #endregion

        public override IEnumerable<User> GetPaging(User model, out long totalCount, string orderBy, string order, int skip, int take)
        {
            var query = $"SELECT COUNT(1) OVER() AS RowsCount, [User].*,(SELECT COUNT(*) FROM [Prediction] WITH(NOLOCK) WHERE UserId = [User].Id) AS NumberOfPredictions,(SELECT SUM(Point) FROM [UserMatchPoint] WITH(NOLOCK) WHERE UserId = [User].Id) AS TotalPoints FROM [User]" +
                        "WHERE 1=1 ";

            if (!string.IsNullOrEmpty(model.UserName))
            {
                query += $"[{nameof(User)}].[{nameof(User.UserName)}]=@{nameof(User.UserName)}";
            }
            if (!string.IsNullOrEmpty(model.NickName))
            {
                query += $"[{nameof(User)}].[{nameof(User.NickName)}]=@{nameof(User.NickName)}";
            }
            if (!string.IsNullOrEmpty(model.Phone))
            {
                query += $"[{nameof(User)}].[{nameof(User.Phone)}]=@{nameof(User.Phone)}";
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                query += $"[{nameof(User)}].[{nameof(User.Email)}]=@{nameof(User.Email)}";
            }
            if (model.Status.HasValue)
            {
                query += $"[{nameof(User)}].[{nameof(User.Status)}]=@{nameof(User.Status)}";
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
                case "Id": result += $"[{nameof(User)}].[Id]"; break;
                case "UserName": result += $"[{nameof(User)}].[UserName]"; break;
                case "NickName": result += $"[{nameof(User)}].[NickName]"; break;
                case "Phone": result += $"[{nameof(User)}].[Phone]"; break;
                case "Email": result += $"[{nameof(User)}].[Email]"; break;
                case "NumberOfPredictions": result += $"[NumberOfPredictions]"; break;
                case "TotalPoints": result += $"[TotalPoints]"; break;
            }

            return result;
        }

        public int CheckEmailCellphoneForInsert(User model)
        {
            var userQuery =
                    $"IF @{nameof(User.Phone)} IS NULL AND @{nameof(User.Email)} IS NULL " +
                    "BEGIN " +
                        "SELECT -5 " +
                        "RETURN " +
                    "END " +
                    $"IF @{nameof(User.Phone)} IS NOT NULL " +
                        $"IF EXISTS(SELECT ID FROM [{nameof(User)}] WHERE {nameof(User.Phone)} = @{nameof(User.Phone)} AND {nameof(User.PhoneVerified)} IS NOT NULL AND {nameof(User.Status)}  = 1) " +
                        "BEGIN " +
                            "SELECT -3 " +
                            "RETURN " +
                        "END " +
                    $"IF @{nameof(User.Email)} IS NOT NULL " +
                        $"IF EXISTS (SELECT ID FROM [{nameof(User)}]  WHERE [{nameof(User.Email)}] = @{nameof(User.Email)} AND [{nameof(User.EmailVerified)}] IS NOT NULL AND {nameof(User.Status)}  = 1) " +
                        "BEGIN " +
                            "SELECT -4 " +
                            "RETURN " +
                        "END " +
                    "SELECT 1 ";
            var userParameters = new DynamicParameters();
            userParameters.Add(nameof(User.Phone), model.Phone);
            userParameters.Add(nameof(User.Email), model.Email);
            return Convert.ToInt32(_repository.ExecuteScalar(userQuery, userParameters));
        }

        public int CheckEmailCellphoneForVerify(User model)
        {
            var userQuery =
                $"IF @{nameof(User.Phone)} IS NULL AND @{nameof(User.Email)} IS NULL " +
                 "BEGIN " +
                    "SELECT -5 " +
                    "RETURN " +
                 "END " +
                $"IF EXISTS(SELECT ID FROM [{nameof(User)}] WHERE {nameof(User.Id)} = @{nameof(User.Id)} AND ([{nameof(User.PhoneVerified)}] IS NOT NULL OR [{nameof(User.PhoneVerified)}] IS NOT NULL)) " +
                 "BEGIN " +
                    "SELECT -6; " +
                    "RETURN " +
                "END " +
                $"IF @{nameof(User.Phone)} IS NOT NULL AND EXISTS(SELECT ID FROM [{nameof(User)}] WHERE {nameof(User.Status)} = 1 AND [{nameof(User.Phone)}] = @{nameof(User.Phone)}) " +
                "BEGIN " +
                    "SELECT -3; " +
                    "RETURN " +
                "END " +
                $"IF @{nameof(User.Email)} IS NOT NULL AND EXISTS(SELECT ID FROM [{nameof(User)}] WHERE {nameof(User.Status)} = 1 AND [{nameof(User.Email)}] = @{nameof(User.Email)}) " +
                "BEGIN " +
                    "SELECT -4; " +
                    "RETURN " +
                "END " +
                "SELECT 1";
            var userParameters = new DynamicParameters();
            userParameters.Add(nameof(User.Phone), model.Phone);
            userParameters.Add(nameof(User.Email), model.Email);
            userParameters.Add(nameof(User.Id), model.Id);
            return Convert.ToInt32(_repository.ExecuteScalar(userQuery, userParameters));
        }

        public IList<SentReceivedSMS> GetUserSentRecievedSms(SentReceivedSMS dataModel, out long totalCount, string orderBy = "CreatedAt", string order = "Desc", int skip = 0, int take = 10)
        {
            var parameters = new DynamicParameters();
            var condition = "1=1 ";
            if (!string.IsNullOrWhiteSpace(dataModel.MSISDN))
            {
                condition += $"AND [{nameof(SentReceivedSMS.MSISDN)}] LIKE @{nameof(SentReceivedSMS.MSISDN)} ";
                parameters.Add(nameof(SentReceivedSMS.MSISDN), $"%{dataModel.MSISDN}%");
            }
            if (!string.IsNullOrWhiteSpace(dataModel.Body))
            {
                condition += $"AND [{nameof(SentReceivedSMS.Body)}] LIKE @{nameof(SentReceivedSMS.Body)} ";
                parameters.Add(nameof(SentReceivedSMS.Body), $"%{dataModel.Body}%");
            }
            if (dataModel.CreatedAt.HasValue)
            {
                condition += $"AND [{nameof(SentReceivedSMS.CreatedAt)}]>=@{nameof(SentReceivedSMS.CreatedAt)}_MIN AND [{nameof(SentReceivedSMS.CreatedAt)}]<=@{nameof(SentReceivedSMS.CreatedAt)}_MAX ";
                parameters.Add($"{nameof(SentReceivedSMS.CreatedAt)}_MIN", (dataModel.CreatedAt.Value.Date));
                parameters.Add($"{nameof(SentReceivedSMS.CreatedAt)}_MAX", (dataModel.CreatedAt.Value.Date.AddDays(1)));
            }
            string query = string.Empty;
            switch (dataModel.IsSent)
            {
                case true:
                    query = $"WITH sentCte ({nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, {nameof(SentReceivedSMS.IsSent)}) AS (" +
                            $"SELECT {nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, 1 FROM dbo.{nameof(SentSms)} WITH(NOLOCK) WHERE {condition}), " +
                            $"totalCte({nameof(SentReceivedSMS.RowNumber)}, {nameof(SentReceivedSMS.RowsCount)}, {nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, {nameof(SentReceivedSMS.IsSent)}) AS(" +
                            $"SELECT ROW_NUMBER() OVER(ORDER BY {orderBy} {order}), (SELECT COUNT(*) FROM sentCte), * FROM sentCte) " +
                            "SELECT * FROM totalCte";
                    break;
                case false:
                    query = $"WITH recievedCte ({nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, {nameof(SentReceivedSMS.IsSent)}) AS (" +
                           $"SELECT {nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, 1 FROM dbo.{nameof(ReceivedSms)} WITH(NOLOCK) WHERE {condition}), " +
                           $"totalCte({nameof(SentReceivedSMS.RowNumber)}, {nameof(SentReceivedSMS.RowsCount)}, {nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, {nameof(SentReceivedSMS.IsSent)}) AS(" +
                           $"SELECT ROW_NUMBER() OVER(ORDER BY {orderBy} {order}), (SELECT COUNT(*) FROM recievedCte), * FROM recievedCte) " +
                           "SELECT * FROM totalCte";
                    break;
                default:
                    query = $"WITH sentCte ({nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, {nameof(SentReceivedSMS.IsSent)}) AS ( " +
                       $"SELECT {nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, 1 FROM dbo.{nameof(SentSms)} WITH(NOLOCK) WHERE {condition}), " +
                       $"mixedCte({nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, {nameof(SentReceivedSMS.IsSent)}) AS( " +
                       $"SELECT {nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, 0 FROM dbo.ReceivedSMS WITH(NOLOCK)  WHERE {condition} UNION ALL SELECT * FROM sentCte), " +
                       $"totalCte ({nameof(SentReceivedSMS.RowNumber)}, {nameof(SentReceivedSMS.RowsCount)}, {nameof(SentReceivedSMS.Body)}, {nameof(SentReceivedSMS.CreatedAt)}, {nameof(SentReceivedSMS.MSISDN)}, {nameof(SentReceivedSMS.IsSent)}) AS( " +
                       $"SELECT ROW_NUMBER() OVER (ORDER BY {orderBy} {order}), (SELECT COUNT(*) FROM mixedCte), * FROM mixedCte)" +
                       $"SELECT * FROM totalCte ";
                    break;
            }
            query += $"ORDER BY {orderBy} {order} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            var result = _smsRepository.Query(query, parameters);
            totalCount = (result.Any() ? result.First().RowsCount : 0);
            return result.ToList();
        }

    }
}