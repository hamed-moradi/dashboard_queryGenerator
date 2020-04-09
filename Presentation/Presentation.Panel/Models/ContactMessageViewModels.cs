using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Model.Entities;
using Asset.Infrastructure.Common;
using System;

namespace Presentation.Panel.Models
{
    public class ContactMessageViewModels : ViewModel
    {
        [Display(Name = "عنوان")]
        [Search]
        public string Subject { get; set; }

        [Display(Name = "پیغام")]
        [Search]
        public string Message { get; set; }

        [Display(Name = "نام")]
        [Search]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی ")]
        [Search]
        public string  LastName { get; set; }

        [Display(Name = "ایمیل")]
        [Search]
        public string  Email { get; set; }

        [Display(Name = "شماره موبایل")]
        [Search]
        public string Mobile { get; set; }

        [Display(Name = "شماره تماس ")]
        [Search]
        public string ContactNumber { get; set; }

        [Display(Name = "اهمیت دارد")]
        [Search]
        public byte?  IsImportant { get; set; }

        [Display(Name = "درخواست دهنده")]
        [Search]
        public int? UserId { get; set; }

        [Display(Name = "آخرین مشاهده")]
        public DateTime? LastSeenAt { get; set; }

        [Display(Name = "آخرین بازدید کننده")]
        public int? LastSeenUserId { get; set; }
        

    }
}