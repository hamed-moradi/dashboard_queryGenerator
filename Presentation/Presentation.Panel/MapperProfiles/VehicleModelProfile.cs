using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;
using MD.PersianDateTime;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class VehicleModelProfile : Profile
    {
        public VehicleModelProfile()
        {
            PersianDateTime tester;
            CreateMap<VehicleModelViewModel, VehicleModel>()
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.DateTypeId, s => s.MapFrom(mf => mf.DateTypeId != null ? (byte)mf.DateTypeId : (byte?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<VehicleModel, VehicleModelViewModel>()
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.DateTypeId, s => s.MapFrom(mf => (GeneralEnums.DateType)mf.DateTypeId))
                .ForMember(d => d.DateTypeTitle, s => s.MapFrom(mf => mf.DateTypeId.HasValue ? EnumHelper<GeneralEnums.DateType>.GetDisplayValue((GeneralEnums.DateType)mf.DateTypeId) : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}