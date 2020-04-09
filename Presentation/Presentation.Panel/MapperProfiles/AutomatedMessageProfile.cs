using Asset.Infrastructure._App;
using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;

namespace Presentation.Panel.MapperProfiles
{
    public class AutomatedMessageProfile : Profile
    {
        public AutomatedMessageProfile()
        {
            CreateMap<AutomatedMessageViewModel, AutomatedMessage>()
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status.HasValue ? (byte)mf.Status : (byte?)null));
            CreateMap<AutomatedMessage, AutomatedMessageViewModel>()
                .ForMember(d => d.UpdatedAt, s => s.MapFrom(mf => mf.UpdatedAt != null ? new PersianDateTime(mf.UpdatedAt).ToString() : "-"))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (GeneralEnums.Status)mf.Status : (GeneralEnums.Status?)null))
                .ForMember(d => d.TypeId, s => s.MapFrom(mf => mf.TypeId != null ? (GeneralEnums.AutomatedMessageType)mf.TypeId : (GeneralEnums.AutomatedMessageType?)null))
                .ForMember(d => d.GroupId, s => s.MapFrom(mf => mf.GroupId != null ? (GeneralEnums.AutomatedMessageGroup)mf.GroupId : (GeneralEnums.AutomatedMessageGroup?)null))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status.Value) : string.Empty))
                .ForMember(d => d.TypeTitle, s => s.MapFrom(mf => mf.TypeId.HasValue ? EnumHelper<GeneralEnums.AutomatedMessageType>.GetDisplayValue((GeneralEnums.AutomatedMessageType)mf.TypeId.Value) : string.Empty))
                .ForMember(d => d.GroupTitle, s => s.MapFrom(mf => mf.GroupId.HasValue ? EnumHelper<GeneralEnums.AutomatedMessageGroup>.GetDisplayValue((GeneralEnums.AutomatedMessageGroup)mf.GroupId.Value) : string.Empty));
        }
    }
}