using System;
using System.Linq;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class RegisteredClientProfile : Profile
    {
        public RegisteredClientProfile()
        {
            CreateMap<RegisteredClientViewModel, RegisteredClient>();
            //.ForMember(d => d.IsActive, s => s.MapFrom(mf => mf.IsActive != null ? (byte)mf.IsActive : (byte?)null));

            CreateMap<RegisteredClient, RegisteredClientViewModel>()
                .ForMember(d => d.IsActiveName, s => s.MapFrom(mf => mf.IsActive.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.IsActive) : string.Empty));
        }
    }
}