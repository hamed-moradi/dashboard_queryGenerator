using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class SettingProfile : Profile
    {
        public SettingProfile()
        {
            CreateMap<SettingViewModel, Setting>();

            CreateMap<Setting, SettingViewModel>();
        }
    }
}