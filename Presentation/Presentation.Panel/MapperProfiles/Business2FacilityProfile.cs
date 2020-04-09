using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;
using System.Collections.Generic;

namespace Presentation.Panel.MapperProfiles
{
    public class Business2FacilityProfile : Profile
    {
        public Business2FacilityProfile()
        {
            CreateMap<Business2FacilityViewModel, Business2Facility>();

            CreateMap<Business2Facility, Business2FacilityViewModel>();
            CreateMap<Business2FacilityViewModel, Business2FacilityItem>();
            CreateMap<Business2FacilityItem, Business2Facility>();
            CreateMap<Business2FacilityViewModel,Business2FacilityItem>();
            CreateMap<List<Business2FacilityViewModel>, Business2FacilityJson>()
                .ForMember(d => d.Items, s => s.MapFrom(mf => mf));
        }
    }
}