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
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            PersianDateTime tester;
            CreateMap<BusinessViewModel, Business>()
                .ForMember(d => d.Location, s => s.MapFrom(mf => mf.Latitude.HasValue && mf.Longitude.HasValue ? SqlGeography.Point(mf.Latitude.Value, mf.Longitude.Value, 4326) : SqlGeography.Point(0, 0, 4326)))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.EvidenceStatusId, s => s.MapFrom(mf => mf.EvidenceStatusId.HasValue ? (byte)mf.EvidenceStatusId : (byte?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status.HasValue ? (byte)mf.Status : (byte?)null))
                .ForMember(d=>d.ChangeRequest,s=>s.MapFrom(mf=>mf.ChangeRequest.HasValue?(byte)mf.ChangeRequest:(byte?)null))
                .ForMember(d => d.AvailabilityStatusId, s => s.MapFrom(mf => mf.AvailabilityStatusId.HasValue ? (byte)mf.AvailabilityStatusId : (byte?)null));

            CreateMap<Business, BusinessViewModel>()
                .ForMember(d => d.Latitude, s => s.MapFrom(mf => mf.Location != null ? (double)mf.Location.Lat : (double?)null))
                .ForMember(d => d.Longitude, s => s.MapFrom(mf => mf.Location != null ? (double)mf.Location.Long : (double?)null))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => mf.CreatedAt != null ? new PersianDateTime(mf.CreatedAt).ToString() : string.Empty))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                .ForMember(d => d.EntityHasChanged, s => s.MapFrom(mf => mf.LastEntityData != null ? true : false))
                .ForMember(d => d.CategoryHasChanged, s => s.MapFrom(mf => mf.LastCategoryData != null ? true : false))
                .ForMember(d => d.FacilityHasChanged, s => s.MapFrom(mf => mf.LastFacilityData != null ? true : false))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (GeneralEnums.Status)mf.Status : (GeneralEnums.Status?)null))
                .ForMember(d=>d.ChangeRequest,s=>s.MapFrom(mf=>mf.ChangeRequest!=null?(EditStatus)mf.ChangeRequest:(EditStatus?)null))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty))
                .ForMember(d => d.EvidenceStatusId, s => s.MapFrom(mf => mf.EvidenceStatusId != null ? (GeneralEnums.EvidenceStatusType)mf.EvidenceStatusId : (GeneralEnums.EvidenceStatusType?)null))
                .ForMember(d => d.EvidenceStatusTitle, s => s.MapFrom(mf => mf.EvidenceStatusId.HasValue ? EnumHelper<GeneralEnums.EvidenceStatusType>.GetDisplayValue((GeneralEnums.EvidenceStatusType)mf.EvidenceStatusId) : string.Empty))
                .ForMember(d => d.AvailabilityStatusId, s => s.MapFrom(mf => mf.AvailabilityStatusId != null ? (GeneralEnums.AvilabilityStatus)mf.AvailabilityStatusId : (GeneralEnums.AvilabilityStatus?)null))
                .ForMember(d => d.AvailabilityStatusTitle, s => s.MapFrom(mf => mf.AvailabilityStatusId.HasValue ? EnumHelper<GeneralEnums.AvilabilityStatus>.GetDisplayValue((GeneralEnums.AvilabilityStatus)mf.AvailabilityStatusId) : string.Empty))
                .ForMember(d => d.ServiceRequest2BusinessStatusTitle, s => s.MapFrom(mf => mf.ServiceRequest2BusinessStatus.HasValue ? EnumHelper<GeneralEnums.ServiceRequest2BusinessStatusType>.GetDisplayValue((GeneralEnums.ServiceRequest2BusinessStatusType)mf.ServiceRequest2BusinessStatus) : string.Empty))
                .ForMember(d => d.GenderTypeId, s => s.MapFrom(mf => mf.GenderTypeId != null ? (GeneralEnums.Gender)mf.GenderTypeId : (GeneralEnums.Gender?)null))
                .ForMember(d => d.GenderTypeString, s => s.MapFrom(mf => mf.GenderTypeId.HasValue ? EnumHelper<GeneralEnums.Gender>.GetDisplayValue((GeneralEnums.Gender)mf.GenderTypeId) : string.Empty));


            CreateMap<XmlBusiness, Business>();
        }
    }
}