using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class Match2ClubProfile : Profile
    {
        public Match2ClubProfile()
        {
            CreateMap<Match2ClubViewModel, Match2Club>();
            CreateMap<Match2Club, Match2ClubViewModel>();
        }
    }
}