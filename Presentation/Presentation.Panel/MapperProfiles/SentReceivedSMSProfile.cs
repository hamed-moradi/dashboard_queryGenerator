using Asset.Infrastructure.Common;
using AutoMapper;
using Domain.Model.Entities;
using MD.PersianDateTime;
using Presentation.Panel.Models;
using System;

namespace Presentation.Panel.MapperProfiles
{
    public class SentReceivedSMSProfile:Profile
    {
        public SentReceivedSMSProfile()
        {
            PersianDateTime persianDt;
            CreateMap<SentReceivedSMSViewModel, SentReceivedSMS>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => PersianDateTime.TryParse(mf.CreatedAt, out persianDt, @"/") ? PersianDateTime.Parse(mf.CreatedAt, @"/").ToDateTime() : (DateTime?)null))
                .ForMember(d => d.IsSent, s => s.MapFrom(mf => mf.SmsType.HasValue ? Convert.ToBoolean(mf.SmsType.Value) : (bool?)null));
            CreateMap<SentReceivedSMS, SentReceivedSMSViewModel>()
                .ForMember(d => d.CreatedAt, s => s.MapFrom(mf => new PersianDateTime(mf.CreatedAt).ToString()))
                .ForMember(d => d.SmsType, s => s.MapFrom(mf => (GeneralEnums.SmsType?)Convert.ToByte(mf.IsSent)));
        }
    }
}