using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Asset.Infrastructure.Common;
using System;

namespace Presentation.Panel.Models
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            if (string.IsNullOrEmpty(PreviousUrl) || !PreviousUrl.ToLower().Contains("index"))
            {
                var url = System.Web.HttpContext.Current.Request.UrlReferrer;
                if (url != null && url.ToString().ToLower().Contains("index"))
                {
                    PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer?.ToString() ?? "";
                }
            }
        }

        [Display(Name = "شناسه")]
        public int? Id { get; set; }
        public int ThisPageIndex { get; set; } = 1;
        public int ThisPageSize { get; set; } = 10;
        public string PageOrder { get; set; } = "Desc";
        public string PageOrderBy { get; set; } = "Id";

        public string PreviousUrl { get; set; }
    }

    public class ViewModel : BaseViewModel
    {
        [Display(Name = "ایجاد کننده")]
        public int? CreatorId { get; set; }

        [Display(Name = "ایجاد کننده")]
        public string CreatorName { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }

        [Display(Name = "آخرین ویرایش کننده")]
        public int? UpdaterId { get; set; }

        [Display(Name = "آخرین ویرایش کننده")]
        public string UpdaterName { get; set; }

        [Display(Name = "تاریخ آخرین ویرایش")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string UpdatedAt { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public virtual EditStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }

    public class DropDownViewModel
    {
        public int id { get; set; }
        public int? parentId { get; set; }
        public string image { get; set; }
        public string name { get; set; }
    }

    public class DropDownModel
    {
        public long TotalCount { get; set; }
        public List<DropDownViewModel> Items { get; set; }
    }

    public class TreeModel
    {
        public int? id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
    }

    public enum EditStatus
    {
        [Display(Name = "غیر فعال")] Inactive = 0,
        [Display(Name = "فعال")] Active = 1
    }
    public enum CategoryEditStatus
    {
        [Display(Name = "غیر فعال")] Inactive = 0,
        [Display(Name = "فعال")] Active = 1,
        [Display(Name = "به زودی")] ComingSoon = 4
    }

    public enum UserEditStatus
    {
        [Display(Name = "غیر فعال")] Inactive = 0,
        [Display(Name = "فعال")] Active = 1,
        [Display(Name = "معلق")] Pending = 2
    }

    public enum EventStatus
    {
        [Display(Name = "برگزارشده")] Held = 1,
        [Display(Name = "برگزارنشده")] NotHeld = 2
    }

    public enum ClubStatus
    {
        [Display(Name = "فعال")] Acticve = 1,
        [Display(Name = "غیرفعال")] DeActive = 2
    }

    public enum MatchesStatus
    {
        //[Display(Name = "آماده")] Ready = 1,
        //[Display(Name = "لغو شده")] Canceled = 2,
        //[Display(Name = "انجام شده")] Done = 3,
        //[Display(Name = "حذف شده")] Deleted = 4

        [Display(Name = "فعال")] Acticve = 1,
        [Display(Name = "غیرفعال")] DeActive = 2
    }

    public enum MatchesStatusForIndex
    {
        //[Display(Name = "آماده")] Ready = 1,
        //[Display(Name = "لغو شده")] Canceled = 2,
        //[Display(Name = "انجام شده")] Done = 3,
        //[Display(Name = "حذف شده")] Deleted = 4

        [Display(Name = "فعال")] Acticve = 1,
        [Display(Name = "غیرفعال")] DeActive = 2,
        [Display(Name = "برگزار شده")] Done = 4
    }

    public enum MessageStatus : byte
    {
        [Display(Name = "تازه")] New = 1,
        [Display(Name = "خوانده شده")] Read = 2
    }

    public enum States
    {
        [Display(Name = "پیش نویس")] Draft = 1,
        [Display(Name = "ارسال شده")] Sent = 2,
        [Display(Name = "دریافت شده")] Deliver = 3,
        [Display(Name = "دیده شده")] Seen = 4
    }
    public enum EvidenceEditStatus
    {
        [Display(Name = "نامشخص")] Unknown = 0,
        [Display(Name = "تایید شده")] Accepted = 1,
        [Display(Name = "رد شده")] Rejected = 2
    }

    public enum ForceUpdateState : byte
    {

        [Display(Name = "غیر ضروری")]
        NotForce = 0,
        [Display(Name = "ضروری")]
        Force = 1
    }

    public enum InitiatorType
    {
        [Display(Name = "مشتری")] Client = 1,
        [Display(Name = "سرویس دهنده")] Provider = 2,
        [Display(Name = "ادمین")] Admin = 3
    }

}