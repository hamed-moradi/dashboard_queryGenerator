using System;
using Asset.Infrastructure._App;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class ServiceRequestPreInvoiceItemProfile : Profile
    {
        public ServiceRequestPreInvoiceItemProfile()
        {
            PersianDateTime tester;
            CreateMap<ServiceRequestPreInvoiceItemViewModel, ServiceRequestPreInvoiceItem>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out tester, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.LastModifiedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.LastModifiedAt, out tester, @"/") ? PersianDateTime.Parse(mf.LastModifiedAt, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<ServiceRequestPreInvoiceItem, ServiceRequestPreInvoiceItemViewModel>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.LastModifiedAt, s => s.MapFrom(mf => mf.LastModifiedAt.HasValue ? new PersianDateTime(mf.LastModifiedAt).ToString() : "-"));
        }
    }
}