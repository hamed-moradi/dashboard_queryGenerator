using Domain.Application.Repository;
using Dapper;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using Asset.Infrastructure._App;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Asset.Infrastructure.Common;

namespace Domain.Application.Services
{
    public class SessionService : BaseService<Session>, ISessionService
    {
        #region Constructor

        private readonly IRepository<Session> _repository;
        private readonly INotificationService _notificationService;
        private readonly IUser2NotificationService _userNotificationService;

        public SessionService(IRepository<Session> repository, INotificationService notificationService, IUser2NotificationService userNotificationService) : base(repository)
        {
            _repository = repository;
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
        }

        #endregion

        #region Private

        public string SendByFCM(string[] deviceIds, NotificationModel notification)
        {
            var result = string.Empty;
            var fcm = new Fcm();
            while (deviceIds.Any())
            {
                var part = deviceIds.Take(1000);
                result += fcm.SendNotification(part.ToArray(), notification);
                deviceIds = deviceIds.Skip(1000).ToArray();
            }
            return result;
        }

        #endregion

        public int ChangeStatusUserId(int userId, byte status)
        {
            var query = $"Update [{nameof(Session)}] Set [{nameof(Session.Status)}]=@{nameof(Session.Status)} WHERE [{nameof(Session.UserId)}]=@{nameof(Session.UserId)}";
            var parameters = new DynamicParameters();
            parameters.Add(nameof(Session.UserId), userId);
            parameters.Add(nameof(Session.Status), status);
            var result = _repository.Execute(query, parameters);
            return result;
        }

        //public string SendNotification(User model, IosNotificationModel notification, int creatorId)
        //{
        //    var query = $"SELECT [{nameof(Session)}].[{nameof(Session.UserId)}],[{nameof(Session)}].[{nameof(Session.FcmId)}] " +
        //        $"FROM [{nameof(User)}] WITH(NOLOCK) " +
        //        $"INNER JOIN [{nameof(Session)}] WITH(NOLOCK) ON [{nameof(User)}].[{nameof(User.Id)}] = [{nameof(Session)}].[{nameof(Session.UserId)}] " +
        //        $"WHERE [{nameof(Session)}].[{nameof(Session.Status)}] = {(byte)GeneralEnums.Status.Active} " +
        //        $"AND [{nameof(Session)}].[{nameof(Session.FcmId)}] IS NOT NULL AND [{nameof(Session)}].[{nameof(Session.FcmId)}] <> '' ";
        //    var parameters = new DynamicParameters();
        //    if (!string.IsNullOrWhiteSpace(model.Phone))
        //    {
        //        query += $"AND [{nameof(User)}].[{nameof(User.Phone)}] = @{nameof(User.Phone)} ";
        //        parameters.Add(nameof(User.Phone), model.Phone);
        //    }
        //    if (!string.IsNullOrWhiteSpace(model.Email))
        //    {
        //        query += $"AND [{nameof(User)}].[{nameof(User.Email)}] = @{nameof(User.Email)} ";
        //        parameters.Add(nameof(User.Email), model.Email);
        //    }
        //    //if (model.IdentityProviderId.HasValue)
        //    //{
        //    //    query += $"AND [{nameof(User)}].[{nameof(User.IdentityProviderId)}]=@{nameof(User.IdentityProviderId)} ";
        //    //    parameters.Add(nameof(User.IdentityProviderId), model.IdentityProviderId);
        //    //}
        //    //if (model.UserTypeId.HasValue)
        //    //{
        //    //    query += $"AND [{nameof(User)}].[{nameof(User.UserTypeId)}]=@{nameof(User.UserTypeId)} ";
        //    //    parameters.Add(nameof(User.UserTypeId), model.UserTypeId);
        //    //}
        //    if (model.CreatedAt.HasValue)
        //    {
        //        query += $"AND [{nameof(User)}].[{nameof(User.CreatedAt)}]>=@{nameof(User.CreatedAt)}_MIN AND [{nameof(User)}].[{nameof(User.CreatedAt)}]<=@{nameof(User.CreatedAt)}_MAX ";
        //        parameters.Add($"{nameof(User.CreatedAt)}_MIN", (model.CreatedAt.Value.Date));
        //        parameters.Add($"{nameof(User.CreatedAt)}_MAX", (model.CreatedAt.Value.Date.AddDays(1)));
        //    }
        //    if (model.UpdatedAt.HasValue)
        //    {
        //        query += $"AND [{nameof(User)}].[{nameof(User.UpdatedAt)}]>=@{nameof(User.UpdatedAt)}_MIN AND [{nameof(User)}].[{nameof(User.UpdatedAt)}]<=@{nameof(User.UpdatedAt)}_MAX ";
        //        parameters.Add($"{nameof(User.UpdatedAt)}_MIN", (model.UpdatedAt.Value.Date));
        //        parameters.Add($"{nameof(User.UpdatedAt)}_MAX", (model.UpdatedAt.Value.Date.AddDays(1)));
        //    }
        //    if (model.Status.HasValue)
        //    {
        //        query += $"AND [{nameof(User)}].[{nameof(User.Status)}]=@{nameof(User.Status)} ";
        //        parameters.Add(nameof(User.Status), model.Status);
        //    }
        //    else
        //    {
        //        query += $"AND [{nameof(User)}].[{nameof(User.Status)}] <> {(byte)GeneralEnums.Status.Deleted} ";
        //    }
        //    string response;
        //    var result = _repository.Query(query, parameters);
        //    if (result.Any())
        //    {
        //        var notificationJson = new Notification
        //        {
        //            Body = JsonConvert.SerializeObject(notification),
        //            UserCount = result.Count(),
        //            CreatorId = creatorId,
        //            Status = (byte)GeneralEnums.NotificationStatus.Draft,
        //        };
        //        var notificationId = _notificationService.Insert(notificationJson);
        //        response = SendByFCM(result.Select(s => s.FcmId.ToString()).ToArray(), notification);
        //        if (response.ToLower().Contains("results"))
        //        {
        //            var sentAt = DateTime.Now;
        //            var userNotifications = new List<User2Notification>();
        //            foreach (var item in result)
        //            {
        //                userNotifications.Add(new User2Notification
        //                {
        //                    NotificationId = notificationId,
        //                    UserId = item.UserId,
        //                    SentAt = sentAt
        //                });
        //            }
        //            _userNotificationService.BulkInsert(userNotifications);
        //        }
        //        else
        //        {
        //            notificationJson.Id = notificationId;
        //            notificationJson.Status = (byte)GeneralEnums.NotificationStatus.Failed;
        //            _notificationService.Update(notificationJson);
        //        }
        //    }
        //    else
        //        response = "شناسه دستگاه های کاربران انتخابی یافت نشد.";
        //    return response;
        //}
    }
}