using System.ComponentModel.DataAnnotations;
using Asset.Infrastructure.Common;
using System;

namespace Presentation.Panel.Models
{
    public class ServiceRequestReviewViewModel : ViewModel
    {
        [Display(Name = "شناسه سرویس")]
        public int? ServiceRequestId { get; set; }

        [Display(Name = "شناسه کسب و کار")]
        public int? ServiceRequest2BusinessId { get; set; }

        [Display(Name = "منتصب به")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public InitiatorType? InitiatorId { get; set; }
        [Display(Name = "منتصب به")]
        public string InitiatorTitle { get; set; }

        [Display(Name = "عنوان")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "امتیاز")]
        [Search]
        public long? Point { get; set; }

        [Display(Name = "متن")]
        public string Body { get; set; }

        public int? FeelingId { get; set; }
        [Display(Name = "میزان رضایت")]
        public string FeelingTitle { get; set; }
        public string FeelingAvatar { get; set; }
        
        [Display(Name = "آی پی")]
        public string IP { get; set; }

        public int? UserCreatorId { get; set; }
        
        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public new UserEditStatus? Status { get; set; }

    }
}