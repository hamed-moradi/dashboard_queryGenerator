using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class ContentCategoryProfile : Profile
    {
        public ContentCategoryProfile()
        {
            PersianDateTime tester;
            CreateMap<ContentCategoryViewModels, ContentCategory>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<ContentCategory, ContentCategoryViewModels>()
                .ForMember(d=>d.Status,s=>s.MapFrom(mf=> (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status) mf.Status) : string.Empty));

            CreateMap<ContentCategory, TreeModel>()
                .ForMember(d => d.id, s => s.MapFrom(mf => mf.Id))
                .ForMember(d => d.text, s => s.MapFrom(mf => mf.Title))
                .ForMember(d => d.parent, s => s.MapFrom(mf => mf.ParentId != null ? mf.ParentId.ToString() : "#"));
        }
    }
}