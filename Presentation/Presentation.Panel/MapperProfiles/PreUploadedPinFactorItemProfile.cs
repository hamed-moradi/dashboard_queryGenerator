using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class PreUploadedPinFactorItemProfile : Profile
    {
        public PreUploadedPinFactorItemProfile()
        {
            PersianDateTime persianDt;
            CreateMap<PreUploadedPinFactorItemViewModel, PreUploadedPinFactorItem>()
                 .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<PreUploadedPinFactorItem, PreUploadedPinFactorItemViewModel>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()));
        }
    }
}