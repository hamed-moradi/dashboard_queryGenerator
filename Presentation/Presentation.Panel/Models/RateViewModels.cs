using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class RateViewModel : BaseViewModel
    {
        [Display(Name = "امتیاز")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد امتیاز خالی است")]
        public decimal? Point { get; set; }

        [Display(Name = "آی پی")]
        public string IP { get; set; }

        [Display(Name = "خدمات دهنده")]
        public int? ProviderId { get; set; }

        [Search]
        [Display(Name = "خدمات دهنده")]
        public string BusinessTitle { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public int? UserCreatorId { get; set; }

        [Display(Name = "کاربر ایجاد کننده")]
        public string UserCreatorName { get; set; }

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
        public EditStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}