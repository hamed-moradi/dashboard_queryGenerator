using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class Category2FacilityProfile : Profile
    {
        public Category2FacilityProfile()
        {
            CreateMap<Category2FacilityViewModel, Category2Facility>();

            CreateMap<Category2Facility, Category2FacilityViewModel>();
        }
    }
}