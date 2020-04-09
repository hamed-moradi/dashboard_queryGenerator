using System;
using Asset.Infrastructure._App;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class ProviderEvidenceDocProfile : Profile
    {
        public ProviderEvidenceDocProfile()
        {
            PersianDateTime tester;
            CreateMap<ProviderEvidenceDocViewModel, ProviderEvidenceDoc>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?) null))
                .ForMember(d => d.AttachmentTypeId, s => s.MapFrom(mf => mf.AttachmentTypeId != null ? (byte)mf.AttachmentTypeId : (byte?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte) mf.Status : (byte?) null));

            CreateMap<ProviderEvidenceDoc, ProviderEvidenceDocViewModel>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.AttachmentTypeId, s => s.MapFrom(mf => (GeneralEnums.ProviderEvidenceDocType)mf.AttachmentTypeId))
                .ForMember(d => d.AttachmentTypeTitle, s => s.MapFrom(mf => mf.AttachmentTypeId.HasValue ? EnumHelper<GeneralEnums.ProviderEvidenceDocType>.GetDisplayValue((GeneralEnums.ProviderEvidenceDocType)mf.AttachmentTypeId) : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (EditStatus)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty));
        }
    }
}