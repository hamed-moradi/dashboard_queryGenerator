using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;
using Asset.Infrastructure._App;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<ClubViewModel, Club>()
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null));

            CreateMap<Club, ClubViewModel>()
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<ClubStatus>.GetDisplayValue((ClubStatus)mf.Status) : string.Empty))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (ClubStatus)mf.Status));
        }
    }
}