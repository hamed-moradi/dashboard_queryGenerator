using System;
using Asset.Infrastructure._App;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class ServiceRequestPreInvoiceProfile : Profile
    {
        public ServiceRequestPreInvoiceProfile()
        {
            PersianDateTime tester;
            CreateMap<ServiceRequestPreInvoiceViewModel, ServiceRequestPreInvoice>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.DueDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.DueDate, out tester, @"/") ? PersianDateTime.Parse(mf.DueDate, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.LastModifiedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.LastModifiedAt, out tester, @"/") ? PersianDateTime.Parse(mf.LastModifiedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (byte?)mf.Status));

            CreateMap<ServiceRequestPreInvoice, ServiceRequestPreInvoiceViewModel>()
                .ForMember(d => d.DueDate, s => s.MapFrom(mf => mf.DueDate.HasValue ? new PersianDateTime(mf.DueDate).ToString() : "-"))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
               .ForMember(d => d.LastModifiedAt, s => s.MapFrom(mf => mf.LastModifiedAt.HasValue ? new PersianDateTime(mf.LastModifiedAt).ToString() : "-"))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.ServiceRequestPreInvoiceStatusType)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => EnumHelper<GeneralEnums.ServiceRequestPreInvoiceStatusType>.GetDisplayValue((GeneralEnums.ServiceRequestPreInvoiceStatusType)mf.Status)));
        }
    }
}