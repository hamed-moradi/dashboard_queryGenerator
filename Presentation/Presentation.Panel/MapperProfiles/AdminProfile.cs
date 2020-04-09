using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            PersianDateTime tester;
            CreateMap<AdminViewModel, Admin>()
                //.ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?) null))
                //.ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?) null))
                .ForMember(d => d.LastLogin, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.LastLogin, out tester, @"/") ? PersianDateTime.Parse(mf.LastLogin, @"/").ToDateTime() : (DateTime?) null))
                .ForMember(d => d.Password, s => s.MapFrom(mf => CommonHelper.Md5Password(mf.Password)))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte) mf.Status : (byte?) null));

            CreateMap<Admin, AdminViewModel>()
                .ForMember(d => d.Password, s => s.MapFrom(mf => "******"))
                .ForMember(d => d.ConfirmPassword, s => s.MapFrom(mf => "******"))
                .ForMember(d => d.GenderTitle, s => s.MapFrom(mf => mf.Gender.HasValue ? EnumHelper<GeneralEnums.Gender>.GetDisplayValue((GeneralEnums.Gender) mf.Gender) : string.Empty))
                //.ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                //.ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.LastLogin, s => s.MapFrom(mf => mf.LastLogin != null ? new PersianDateTime(mf.LastLogin).ToString() : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status) mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty))
                .ForMember(d => d.CreatorName, s => s.MapFrom(mf => mf.FullName))
                .ForMember(d => d.UpdaterName, s => s.MapFrom(mf => mf.FullName));

            CreateMap<ChangePasswordViewModel, Admin>()
                .ForMember(d => d.Password, s => s.MapFrom(mf => CommonHelper.Md5Password(mf.Password)));

            CreateMap<Admin, ChangePasswordViewModel>()
                .ForMember(d => d.CurrentPassword, s => s.MapFrom(mf => "******"))
                .ForMember(d => d.Password, s => s.MapFrom(mf => string.Empty))
                .ForMember(d => d.ConfirmPassword, s => s.MapFrom(mf => string.Empty));
        }
    }
}