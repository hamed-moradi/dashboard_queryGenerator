﻿using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            PersianDateTime tester;
            CreateMap<RoleViewModel, Role>()
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<Role, RoleViewModel>()
                //.ForMember(d => d.ParentName, s => s.MapFrom(mf => mf.Parent != null ? mf.Parent.Title : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}