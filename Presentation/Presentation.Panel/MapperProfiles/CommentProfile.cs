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
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            PersianDateTime persianDt;
            CreateMap<CommentViewModel, Comment>()
                .ForMember(d => d.CreatedAt,s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/")? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime(): (DateTime?)null))
                .ForMember(d => d.Status, s => s.MapFrom(mf => mf.Status != null ? (byte)mf.Status : (byte?)null))
                .ForMember(d => d.CommentEntityTypeId, s => s.MapFrom(mf => mf.CommentEntityTypeId!= null ? (byte)mf.CommentEntityTypeId : (byte?)null))
                ;

            CreateMap<Comment, CommentViewModel>()
                .ForMember(d => d.Body, s => s.MapFrom(mf => mf.Body.Length > 100 ? $"{mf.Body.Take(75)} ..." : mf.Body))
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.Status, s => s.MapFrom(mf => (GeneralEnums.Status)mf.Status))
                .ForMember(d => d.StatusTitle, s => s.MapFrom(mf => mf.Status.HasValue ? EnumHelper<GeneralEnums.Status>.GetDisplayValue((GeneralEnums.Status)mf.Status) : string.Empty))
                .ForMember(d => d.CommentEntityTypeId, s => s.MapFrom(mf => (GeneralEnums.CommentEntityType)mf.CommentEntityTypeId))
                .ForMember(d => d.CommentEntityTypeTitle, s => s.MapFrom(mf => mf.CommentEntityTypeId.HasValue ? EnumHelper<GeneralEnums.CommentEntityType>.GetDisplayValue((GeneralEnums.CommentEntityType)mf.CommentEntityTypeId) : string.Empty));
        }
    }
}