using Domain.Application.Services;
using Domain.Model._App;
using SimpleInjector;

namespace Domain.Application._App
{
    public class ModuleInjector
    {
        public static void Init(Container container)
        {
            Dapper.EntityFramework.Handlers.Register();
            container.Register(typeof(Repository.IRepository<>), typeof(Repository.Repository<>), Lifestyle.Singleton);
            container.Register(() => new ConnectionKeeper(), Lifestyle.Singleton);
            container.Register<IAdminService, AdminService>();
            container.Register<IEventService, EventService>();
            container.Register<IClubService, ClubService>();
            container.Register<IMatchService, MatchService>();
            container.Register<IMatchGroupService, MatchGroupService>();
            container.Register<IMatch2ClubService, Match2ClubService>();
            container.Register<IPredictionService, PredictionService>();
            container.Register<IPredictionStatisticsService, PredictionStatisticsService>();
            container.Register<IPermissionService, PermissionService>();
            container.Register<IRoleService, RoleService>();
            container.Register<ISessionService, SessionService>();
            container.Register<IModuleService, ModuleService>();
            container.Register<IUserService, UserService>();
            container.Register<IUserLeaderBoardService, UserLeaderBoardService>();
            container.Register<IDropDownService, DropDownService>();
            container.Register<IPageService, PageService>();
            container.Register<INotificationService, NotificationService>();
            container.Register<IUser2NotificationService, User2NotificationService>();
            container.Register<ISentSmsService, SentSmsService>();
            container.Register<IReceivedSmsService, ReceivedSmsService>();
            container.Register<IAppSettingService, AppSettingService>();
            container.Register<IAppSetting2AppSettingGroupService, AppSetting2AppSettingGroupService>();
            container.Register<IAppSettingGroupService, AppSettingGroupService>();
        }
    }
}