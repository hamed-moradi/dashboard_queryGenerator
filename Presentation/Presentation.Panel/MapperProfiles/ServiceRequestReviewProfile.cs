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
    public class ServiceRequestReviewProfile : Profile
    {
        public ServiceRequestReviewProfile()
        {
            PersianDateTime tester;
            CreateMap<ServiceRequestReviewViewModel, ServiceRequestReview>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.InitiatorTypeId, s => s.MapFrom(mf => mf.InitiatorId.HasValue ? (byte)mf.InitiatorId : (byte?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status.HasValue ? (byte)mf.Status : (byte?)null));

            CreateMap<ServiceRequestReview, ServiceRequestReviewViewModel>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => mf.CreatedAt != null ? new PersianDateTime(mf.CreatedAt).ToString() : string.Empty))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));

        }
    }
}