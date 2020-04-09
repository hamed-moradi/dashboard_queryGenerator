using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class ContentProfile : Profile
    {
        public ContentProfile()
        {
            PersianDateTime tester;
            CreateMap<ContentViewModel, Content>()
                .ForMember(d => d.StartShowDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.StartShowDate, out tester, @"/") ? PersianDateTime.Parse(mf.StartShowDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.EndShowDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.EndShowDate, out tester, @"/") ? PersianDateTime.Parse(mf.EndShowDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?) null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<Content, ContentViewModel>()
                .ForMember(d => d.StartShowDate, s => s.MapFrom(mf => mf.StartShowDate.HasValue ? new PersianDateTime(mf.StartShowDate).ToString() : string.Empty))
                .ForMember(d => d.EndShowDate, s => s.MapFrom(mf => mf.EndShowDate.HasValue ? new PersianDateTime(mf.EndShowDate).ToString() : string.Empty))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}