using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;
using System.Collections.Generic;

namespace Presentation.Panel.MapperProfiles
{
    public class Category2BusinessProfile : Profile
    {
        public Category2BusinessProfile()
        {
            CreateMap<Category2BusinessViewModel, Category2Business>();
            CreateMap<Category2Business, Category2BusinessViewModel>();
            CreateMap<Category2BusinessItem, Category2Business>();
            CreateMap<Category2BusinessViewModel, Category2BusinessItem>();
            CreateMap<List<Category2BusinessViewModel>, Category2BusinessJson>()
                .ForMember(d => d.Items, s => s.MapFrom(mf => mf));
        }
    }
}