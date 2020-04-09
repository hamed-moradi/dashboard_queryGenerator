using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class AutomatedMessageParameterProfile: Profile
    {
        public AutomatedMessageParameterProfile()
        {
            CreateMap<AutomatedMessageParameterViewModel, AutomatedMessageParameter>();
            CreateMap<AutomatedMessageParameter, AutomatedMessageParameterViewModel>();
        }
    }
}