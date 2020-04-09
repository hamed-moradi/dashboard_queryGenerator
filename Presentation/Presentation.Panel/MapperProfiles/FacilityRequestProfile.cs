using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Microsoft.SqlServer.Types;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class FacilityRequestProfile : Profile
    {
        public FacilityRequestProfile()
        {
            PersianDateTime tester;
            CreateMap<FacilityRequestViewModel, ServiceRequest>()
                .ForMember(d => d.Location, s => s.MapFrom(mf => mf.Latitude.HasValue && mf.Longitude.HasValue ? SqlGeography.Point(mf.Latitude.Value, mf.Longitude.Value, 4326) : SqlGeography.Point(0, 0, 4326)))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.StartDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.StartDate, out tester, @"/") ? PersianDateTime.Parse(mf.StartDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.EndDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.EndDate, out tester, @"/") ? PersianDateTime.Parse(mf.EndDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<ServiceRequest, FacilityRequestViewModel>()
                .ForMember(d => d.Latitude, s => s.MapFrom(mf => (double)mf.Location.Lat))
                .ForMember(d => d.Longitude, s => s.MapFrom(mf => (double)mf.Location.Long))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.StartDate, s => s.MapFrom(mf => new PersianDateTime(mf.StartDate).ToString("yyyy/MM/dd")))
                .ForMember(d => d.EndDate, s => s.MapFrom(mf => new PersianDateTime(mf.EndDate).ToString("yyyy/MM/dd")))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.ServiceRequestStatusType>.GetDisplayValue((GeneralEnums.ServiceRequestStatusType)mf.Status) : string.Empty));
        }
    }
}