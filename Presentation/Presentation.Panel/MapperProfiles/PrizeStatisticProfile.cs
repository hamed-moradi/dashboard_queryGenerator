using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class PrizeStatisticProfile : Profile
    {
        public PrizeStatisticProfile()
        {
            CreateMap<PrizeStatisticViewModel, PrizeStatistic>();
            CreateMap<PrizeStatistic, PrizeStatisticViewModel>();
        }
    }
}