using System;
using Asset.Infrastructure._App;

using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.MapperProfiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            PersianDateTime persianDt;
            CreateMap<MessageViewModel, Message>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?) null))
                .ForMember(d => d.LastSeenAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.LastSeenAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.LastSeenAt, @"/").ToDateTime() : (DateTime?) null));
            //.ForMember(d => d.User, s => s.MapFrom(mf => new UserModel { NickName = mf.UserName }))
            //.ForMember(d => d.LastSeenUser, s => s.MapFrom(mf => new UserModel { NickName = mf.LastSeener }));

            CreateMap<Message, MessageViewModel>()
                //.ForMember(d => d.UserName, s => s.MapFrom(mf => mf.User != null ? mf.User.NickName : string.Empty))
                //.ForMember(d => d.LastSeener, s => s.MapFrom(mf => mf.LastSeenUser != null ? mf.LastSeenUser.NickName : string.Empty))
                .ForMember(d => d.LastSeenAt, s => s.MapFrom(mf => new PersianDateTime(mf.LastSeenAt).ToString()))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.MessageStatus>.GetDisplayValue((GeneralEnums.MessageStatus) mf.Status) : string.Empty));
        }
    }
}