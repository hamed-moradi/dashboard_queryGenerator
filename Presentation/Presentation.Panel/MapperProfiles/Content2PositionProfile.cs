using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class Content2PositionProfile : Profile
    {
        public Content2PositionProfile()
        {
            CreateMap<Content2PositionViewModel, Content2Position>();

            CreateMap<Content2Position, Content2PositionViewModel>();
        }
    }
}