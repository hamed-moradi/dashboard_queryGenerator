using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class MatchProfile : Profile
    {
        public MatchProfile()
        {
            PersianDateTime persianDt;

            CreateMap<MatchViewModel, Match>()
                .ForMember(d => d.OccurrenceDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.OccurrenceDate, out persianDt, @"/") ? PersianDateTime.Parse(mf.OccurrenceDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.PredictionDeadline, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.PredictionDeadline, out persianDt, @"/") ? PersianDateTime.Parse(mf.PredictionDeadline, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.StatusForIndex != null ? (byte)mf.StatusForIndex : (mf.Status != null ? (byte)mf.Status : (byte?)null)));

            CreateMap<Match, MatchViewModel>()
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<MatchesStatus>.GetDisplayValue((MatchesStatus)mf.Status) : string.Empty))
                .ForMember(d => d.StatusTitleForIndex, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<MatchesStatusForIndex>.GetDisplayValue((MatchesStatusForIndex)mf.Status) : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (MatchesStatus)mf.Status))
                .ForMember(d => d.StatusForIndex, s => s.MapFrom(mf => (MatchesStatusForIndex)mf.Status))
                .ForMember(d => d.PredictionDeadline, s => s.MapFrom(mf => mf.PredictionDeadline != null ? new PersianDateTime(mf.PredictionDeadline).ToString() : string.Empty))
                .ForMember(d => d.OccurrenceDate, s => s.MapFrom(mf => mf.OccurrenceDate != null ? new PersianDateTime(mf.OccurrenceDate).ToString() : string.Empty));
        }
    }
}