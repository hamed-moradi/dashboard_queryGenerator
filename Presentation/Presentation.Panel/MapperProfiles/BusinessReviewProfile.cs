using System;
using System.Linq;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class BusinessReviewProfile : Profile
    {
        public BusinessReviewProfile()
        {
            PersianDateTime persianDt;
            CreateMap<ReviewViewModel, BusinessReview>()
                .ForMember(d => d.CreatedAt,s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/")? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime(): (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<BusinessReview, ReviewViewModel>()
                .ForMember(d => d.Body, s => s.MapFrom(mf => mf.Body.Length > 100 ? $"{mf.Body.Take(75)} ..." : mf.Body))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status) mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}