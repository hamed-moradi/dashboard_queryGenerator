using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;
using System.Data.Entity.Spatial;
using Microsoft.SqlServer.Types;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class FacilityRequestItemProfile : Profile
    {
        public FacilityRequestItemProfile()
        {
            CreateMap<FacilityRequestItemViewModel, ServiceRequestItem>()
                .ForMember(d => d.ServiceRequestId, s => s.MapFrom(mf => mf.FacilityRequestId))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<ServiceRequestItem, FacilityRequestItemViewModel>()
                .ForMember(d => d.FacilityRequestId, s => s.MapFrom(mf => mf.ServiceRequestId))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}