using AutoMapper;
using Presentation.Panel.MapperProfiles;

namespace Presentation.Panel.Components
{
    public class MapperConfig
    {
        public MapperConfiguration Init()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddProfile<Domain.Application._App.ApplicationModelsProfiles>();
                mc.AddProfile<DropDownProfile>();
                mc.AddProfile<AdminProfile>();
                mc.AddProfile<MatchProfile>();
                mc.AddProfile<PredictionProfile>();
                mc.AddProfile<Match2ClubProfile>();
                mc.AddProfile<ClubProfile>();
                mc.AddProfile<EventProfile>();
                mc.AddProfile<RoleProfile>();
                mc.AddProfile<ModuleProfile>();
                mc.AddProfile<PageProfile>();
                mc.AddProfile<PermissionProfile>();
                mc.AddProfile<NotificationProfile>();
                mc.AddProfile<UserProfile>();
                mc.AddProfile<UserLeaderBoardProfile>();
                mc.AddProfile<User2NotificationProfile>();
                mc.AddProfile<SentReceivedSMSProfile>();
                mc.AddProfile<AppSettingProfile>();
                mc.AddProfile<AppSetting2AppSettingGroupProfile>();
                mc.AddProfile<AppSettingGroupProfile>();
                mc.AddProfile<MatchGroupProfile>();
            });
        }
    }
}