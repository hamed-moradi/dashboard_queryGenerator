using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class ContentAttachmentProfile : Profile
    {
        public ContentAttachmentProfile()
        {
            PersianDateTime tester;
            CreateMap<ContentAttachmentViewModel, ContentAttachment>()
                //.ForMember(d => d.AttachmentType, s => s.MapFrom(mf => mf.AttachmentType != null ? (byte)mf.AttachmentType : (byte?)null))
                //.ForMember(d => d.QualityType, s => s.MapFrom(mf => mf.QualityType != null ? (byte)mf.QualityType : (byte?)null))
                //.ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<ContentAttachment, ContentAttachmentViewModel>()
                .ForMember(d => d.AttachmentTypeTitle, s => s.MapFrom(mf => mf.TypeId.HasValue ? EnumHelper<GeneralEnums.AttachmentType>.GetDisplayValue((GeneralEnums.AttachmentType)mf.TypeId) : string.Empty))
                .ForMember(d => d.QualityTypeTitle, s => s.MapFrom(mf => mf.QualityId.HasValue ? EnumHelper<GeneralEnums.QulaityType>.GetDisplayValue((GeneralEnums.QulaityType)mf.QualityId) : string.Empty))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                //.ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}