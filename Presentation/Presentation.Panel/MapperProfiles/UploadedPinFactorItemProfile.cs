using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class UploadedPinFactorItemProfile: Profile
    {
        public UploadedPinFactorItemProfile()
        {
            PersianDateTime persianDt;
            CreateMap<UploadedPinFactorItemViewModel, UploadedPinFactorItem>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<UploadedPinFactorItem, UploadedPinFactorItemViewModel>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : "-"))
                .ForMember(d => d.StatusString, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.FactorItemStatus>.GetDisplayValue((GeneralEnums.FactorItemStatus)mf.Status.Value) : string.Empty));
        }
    }
}