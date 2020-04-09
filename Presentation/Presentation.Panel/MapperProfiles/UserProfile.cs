using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            PersianDateTime tester;
            CreateMap<UserViewModel, User>()
                .ForMember(d => d.Password, s => s.MapFrom(mf => CommonHelper.Md5Password(mf.Password)))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<User, UserViewModel>()
                .ForMember(d => d.Password, s => s.MapFrom(mf => "******"))
                .ForMember(d => d.ConfirmPassword, s => s.MapFrom(mf => "******"))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.IdentityProviderTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.IdentityProvider>.GetDisplayValue((GeneralEnums.IdentityProvider)mf.Status) : string.Empty))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}