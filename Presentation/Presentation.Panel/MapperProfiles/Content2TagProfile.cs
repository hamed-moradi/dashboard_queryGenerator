using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class Content2TagProfile : Profile
    {
        public Content2TagProfile()
        {
            CreateMap<Content2TagViewModel, Content2Tag>();

            CreateMap<Content2Tag, Content2TagViewModel>();
        }
    }
}