using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class PrizeInfoProfile : Profile
    {
        public PrizeInfoProfile()
        {
            PersianDateTime tester;
            CreateMap<PrizeInfoViewModel, PrizeInfo>()
                .ForMember(d => d.CheckoutDate, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CheckoutDate, out tester, @"/") ? PersianDateTime.Parse(mf.CheckoutDate, @"/").ToDateTime() : (DateTime?)null));

            CreateMap<PrizeInfo, PrizeInfoViewModel>()
                .ForMember(d => d.CheckoutDate, s => s.MapFrom(mf => new PersianDateTime(mf.CheckoutDate).ToString()));
        }
    }
}