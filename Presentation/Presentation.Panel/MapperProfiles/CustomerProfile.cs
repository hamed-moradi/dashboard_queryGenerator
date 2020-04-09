using System;
using Asset.Infrastructure._App;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            PersianDateTime tester;
            CreateMap<CustomerViewModel, User>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.LastLoginAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.LastLoginAt, out tester, @"/") ? PersianDateTime.Parse(mf.LastLoginAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.CellPhoneVerifiedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CellPhoneVerifiedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CellPhoneVerifiedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.EmailVerifiedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.EmailVerifiedAt, out tester, @"/") ? PersianDateTime.Parse(mf.EmailVerifiedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.Password, s => s.MapFrom(mf => Encryption.GetMd5Hash(mf.Password + Encryption.PasswordSalt)))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<User, CustomerViewModel>()
                .ForMember(d => d.Password, s => s.MapFrom(mf => "******"))
                .ForMember(d => d.ConfirmPassword, s => s.MapFrom(mf => "******"))
                .ForMember(d => d.GenderTitle, s => s.MapFrom(mf => mf.GenderId.HasValue ? EnumHelper<GeneralEnums.Gender>.GetDisplayValue((GeneralEnums.Gender)mf.GenderId) : string.Empty))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.CellPhoneVerifiedAt, s => s.MapFrom(mf => mf.CellPhoneVerifiedAt != null ? new PersianDateTime(mf.CellPhoneVerifiedAt).ToString() : string.Empty))
                .ForMember(d => d.EmailVerifiedAt, s => s.MapFrom(mf => mf.EmailVerifiedAt != null ? new PersianDateTime(mf.EmailVerifiedAt).ToString() : string.Empty))
                .ForMember(d => d.LastLoginAt, s => s.MapFrom(mf => mf.LastLoginAt != null ? new PersianDateTime(mf.LastLoginAt).ToString() : string.Empty))
                .ForMember(d => d.IdentityProviderId, s => s.MapFrom(mf => (GeneralEnums.IdentityProvider)mf.IdentityProviderId))
                .ForMember(d => d.IdentityProviderTitle, s => s.MapFrom(mf => mf.IdentityProviderId.HasValue ? EnumHelper<GeneralEnums.IdentityProvider>.GetDisplayValue((GeneralEnums.IdentityProvider)mf.IdentityProviderId) : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}