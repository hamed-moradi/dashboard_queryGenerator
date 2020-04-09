using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class BusinessAttachmentProfile : Profile
    {
        public BusinessAttachmentProfile()
        {
            PersianDateTime tester;
            CreateMap<BusinessAttachmentViewModels, BusinessAttachment>()
                .ForMember(d => d.TypeId, s => s.MapFrom(mf => mf.TypeId != null ? (byte)mf.TypeId : (byte?)null))
                .ForMember(d => d.QualityId, s => s.MapFrom(mf => mf.QualityId != null ? (byte)mf.QualityId : (byte?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.UpdatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.UpdatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<BusinessAttachment, BusinessAttachmentViewModels>()
                .ForMember(d => d.TypeId, s => s.MapFrom(mf => (GeneralEnums.AttachmentType)mf.TypeId))
                .ForMember(d => d.TypeTitle, s => s.MapFrom(mf => mf.TypeId.HasValue ? EnumHelper<GeneralEnums.AttachmentType>.GetDisplayValue((GeneralEnums.AttachmentType)mf.TypeId) : string.Empty))
                .ForMember(d => d.QualityId, s => s.MapFrom(mf => mf.QualityId.HasValue ? (GeneralEnums.QulaityType)mf.QualityId.Value : (GeneralEnums.QulaityType?)null))
                .ForMember(d => d.QualityTypeTitle, s => s.MapFrom(mf => mf.QualityId.HasValue ? EnumHelper<GeneralEnums.QulaityType>.GetDisplayValue((GeneralEnums.QulaityType)mf.QualityId) : string.Empty))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}