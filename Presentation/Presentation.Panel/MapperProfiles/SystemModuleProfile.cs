using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class ModuleProfile : Profile
    {
        public ModuleProfile()
        {
            PersianDateTime tester;
            CreateMap<ModuleViewModel, Module>()
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status.HasValue ? (byte) mf.Status : (byte?) null));

            CreateMap<Module, ModuleViewModel>()
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status) mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}