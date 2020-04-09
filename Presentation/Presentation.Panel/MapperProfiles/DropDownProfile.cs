using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class DropDownProfile : Profile
    {
        public DropDownProfile()
        {
            CreateMap<DropDownItemModel, DropDownViewModel>()
                .ForMember(d => d.id, s => s.MapFrom(mf => mf.Id))
                .ForMember(d => d.image, s => s.MapFrom(mf => mf.Image))
                .ForMember(d => d.name, s => s.MapFrom(mf => mf.Name))
                .ForMember(d => d.parentId, s => s.MapFrom(mf => mf.ParentId));

            //CreateMap<Provider, DropDownViewModel>()
            //    .ForMember(d => d.id, s => s.MapFrom(mf => mf.Id))
            //    //.ForMember(d => d.image, s => s.MapFrom(mf => mf.Image))
            //    .ForMember(d => d.name, s => s.MapFrom(mf => $"{mf.FirstName} {mf.LastName}"));
        }
    }
}