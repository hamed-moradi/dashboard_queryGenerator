using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class AppSetting2AppSettingGroupProfile : Profile
    {
        public AppSetting2AppSettingGroupProfile()
        {
            
            CreateMap<AppSetting2AppSettingGroupViewModel, AppSetting2AppSettingGroup>();

            CreateMap<AppSetting2AppSettingGroup, AppSetting2AppSettingGroupViewModel>();
        }
    }
}