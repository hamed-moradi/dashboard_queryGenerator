using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System.Net;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            PersianDateTime tester;
            CreateMap<PageViewModel, Page>()
                .ForMember(d => d.Body, s => s.MapFrom(mf => WebUtility.HtmlEncode(mf.Body)))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<Page, PageViewModel>()
                .ForMember(d => d.Body, s => s.MapFrom(mf => WebUtility.HtmlDecode(mf.Body)))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}