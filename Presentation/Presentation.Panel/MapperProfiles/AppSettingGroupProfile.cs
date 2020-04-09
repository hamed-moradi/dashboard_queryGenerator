using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class AppSettingGroupProfile : Profile
    {
        public AppSettingGroupProfile()
        {
            
            CreateMap<AppSettingGroupViewModel, AppSettingGroup>();

            CreateMap<AppSettingGroup, AppSettingGroupViewModel>();
        }
    }
}