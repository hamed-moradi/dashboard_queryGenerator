using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionViewModel, Role2Module>();

            CreateMap<Role2Module, PermissionViewModel>();
        }
    }
}