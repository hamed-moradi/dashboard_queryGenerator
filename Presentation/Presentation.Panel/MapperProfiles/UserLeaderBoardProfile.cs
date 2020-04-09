using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class UserLeaderBoardProfile : Profile
    {
        public UserLeaderBoardProfile()
        {
            CreateMap<UserLeaderBoardViewModel, UserLeaderBoard>();
            CreateMap<UserLeaderBoard, UserLeaderBoardViewModel>();
        }
    }
}