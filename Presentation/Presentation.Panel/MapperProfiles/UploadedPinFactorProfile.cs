using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class UploadedPinFactorProfile: Profile
    {
        public UploadedPinFactorProfile()
        {
            PersianDateTime persianDt;
            CreateMap<UploadedPinFactorViewModel, UploadedPinFactor>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<UploadedPinFactor, UploadedPinFactorViewModel>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.StatusString, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.PinFactorStatus>.GetDisplayValue((GeneralEnums.PinFactorStatus)mf.Status.Value) : string.Empty));
        }
    }
}