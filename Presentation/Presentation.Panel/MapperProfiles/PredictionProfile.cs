using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class PredictionProfile : Profile
    {
        public PredictionProfile()
        {
            PersianDateTime persianDt;

            CreateMap<PredictionViewModel, Prediction>()
                .ForMember(d => d.FromDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.FromDate, out persianDt, @"/") ? PersianDateTime.Parse(mf.FromDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.ToDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.ToDate, out persianDt, @"/") ? PersianDateTime.Parse(mf.ToDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<Prediction, PredictionViewModel>()
                .ForMember(d => d.FromDate, s => s.MapFrom(mf => mf.FromDate != null ? new PersianDateTime(mf.FromDate).ToString() : string.Empty))
                .ForMember(d => d.ToDate, s => s.MapFrom(mf => mf.ToDate != null ? new PersianDateTime(mf.ToDate).ToString() : string.Empty))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => mf.CreatedAt != null ? new PersianDateTime(mf.CreatedAt).ToString() : string.Empty));

            CreateMap<PredictionStatistics, PredictionStatisticsViewModel>();
        }
    }
}