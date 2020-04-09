using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using Domain.Application.BaseService;
using Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Asset.Infrastructure.Common.GeneralEnums;

namespace Domain.Application
{
    public interface IAdminService : IBaseService<Admin>
    {
        Admin GetByIdIncludeRole(int id);

        Admin GetByCellPhone(string cellPhone);

        Admin SignIn(string username, string password);

        int ChangePassword(Admin model);

        int SetLoginDate(Admin model);
    }

    public interface IEventService : IBaseService<Event>
    {
    }

    public interface IClubService : IBaseService<Club>
    {
    }

    public interface IPredictionService : IBaseService<Prediction>
    {
    }

    public interface IPredictionStatisticsService : IBaseService<PredictionStatistics>
    {
        PredictionStatistics GetStatistics(Prediction model);
    }

    public interface IMatchService : IBaseService<Match>
    {
        bool CheckExist(Match match, int? id);
    }
    public interface IMatchGroupService : IBaseService<MatchGroup>
    {
    }

    public interface IMatch2ClubService : IBaseService<Match2Club>
    {
        int SetScore(Match2Club model, int adminId, string data = "");
    }

    public interface IDropDownService : IBaseService<DropDownItemModel>
    {
        IEnumerable<DropDownItemModel> GetRoles();
        IEnumerable<DropDownItemModel> GetEvents();
        IEnumerable<DropDownItemModel> GetMatches(int eventId, string key = "");
        IEnumerable<DropDownItemModel> GetClubs(int eventId, string key = "");
        IEnumerable<DropDownItemModel> GetEvents(string key, int skip, int take, out long totalCount);
        IEnumerable<DropDownItemModel> GetMatchGroupes(int eventId, string key = "");
    }



    public interface INotificationService : IBaseService<Notification>
    {
    }

    public interface IPageService : IBaseService<Page>
    {
    }

    public interface IPermissionService : IBaseService<Role2Module>
    {
        IEnumerable<Role2Module> GetByRoleId(int roleId);

        int DeleteByRoleId(int roleId);
    }


    public interface IProviderService : IBaseService<User>
    {
        new void Delete(User model);
        int Insert(User model, byte evidenceStatusId);
    }



    public interface IRoleService : IBaseService<Role>
    {
        Role GetByIdIncludePermissions(int id);

        int SavePermissions(int roleId, IList<Role2Module> permissions);
    }

    public interface ISessionService : IBaseService<Session>
    {
        int ChangeStatusUserId(int userId, byte status);

        //string SendNotification(User model, IosNotificationModel notification, int creatorId);
    }


    public interface IModuleService : IBaseService<Module>
    {
        IEnumerable<Module> GetByRoleId(int roleId);

        IEnumerable<Module> GetByAdminId(int adminId);

        IEnumerable<Module> GetParents();
    }

    public interface IUser2NotificationService : IBaseService<User2Notification>
    {
    }

    public interface IUserService : IBaseService<User>
    {
        int CheckEmailCellphoneForInsert(User model);
        int CheckEmailCellphoneForVerify(User model);
        IList<SentReceivedSMS> GetUserSentRecievedSms(SentReceivedSMS model, out long totalCount, string orderBy = "CreatedAt", string order = "Desc", int skip = 0, int take = 10);
    }

    public interface IUserLeaderBoardService : IBaseService<UserLeaderBoard>
    {
        IList<UserLeaderBoard> GetTotalPoints(UserLeaderBoard model, out long totalCount, string orderBy = "TotalPoints", string order = "Desc", int skip = 0, int take = 10);
    }

    public interface ISentSmsService : IBaseService<SentSms>
    {
    }

    public interface IReceivedSmsService : IBaseService<ReceivedSms>
    {
    }

    public interface IMustToSendSmsService : IDisposable
    {
        Task<(SmsSendErrorCode Status, string Body)> SendAsync(string phone, string uniqueKey, SMSGeteway? getway = null, Dictionary<string, string> dictionary = null);
    }

    public interface IAppSettingService : IBaseService<AppSetting>
    {
        IEnumerable<AppSetting> UpdateByKey(AppSetting model);
    }

    public interface IAppSetting2AppSettingGroupService : IBaseService<AppSetting2AppSettingGroup>
    {
    }

    public interface IAppSettingGroupService : IBaseService<AppSettingGroup>
    {
    }

}