using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            PersianDateTime persianDt;

            CreateMap<EventViewModel, Event>()
                .ForMember(d => d.EndedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.EndedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.EndedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.StartedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.StartedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.StartedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<Event, EventViewModel>()
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<EventStatus>.GetDisplayValue((EventStatus)mf.Status) : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (EventStatus)mf.Status))
                .ForMember(d => d.EndedAt, s => s.MapFrom(mf => mf.EndedAt != null ? new PersianDateTime(mf.EndedAt).ToString() : string.Empty))
                .ForMember(d => d.StartedAt, s => s.MapFrom(mf => mf.StartedAt != null ? new PersianDateTime(mf.StartedAt).ToString() : string.Empty));
        }
    }
}