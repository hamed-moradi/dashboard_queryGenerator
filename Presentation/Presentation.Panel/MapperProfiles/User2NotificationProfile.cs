using AutoMapper;
using Domain.Model.Entities;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class User2NotificationProfile : Profile
    {
        public User2NotificationProfile()
        {
            CreateMap<User2NotificationViewModel, User2Notification>();

            CreateMap<User2Notification, User2NotificationViewModel>();
        }
    }
}