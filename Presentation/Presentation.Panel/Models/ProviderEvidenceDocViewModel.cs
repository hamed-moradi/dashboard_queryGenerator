using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class ProviderEvidenceDocViewModel : BaseViewModel
    {
        [Display(Name = "خدمات دهنده")]
        [Required(ErrorMessage = "خدمات دهنده را مشخص نمایید")]
        public int? ProviderId { get; set; }

        [Search]
        [Display(Name = "خدمات دهنده")]
        public string ProviderName { get; set; }

        [Display(Name = "کسب و کار")]
        public int? BusinessId { get; set; }

        [Search]
        [Display(Name = "کسب و کار")]
        public string BusinessTitle { get; set; }

        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "فیلد عنوان خالی است")]
        public byte? Priority { get; set; }

        [Display(Name = "نوع پیوست")]
        [Required(ErrorMessage = "نوع پیوست را مشخص کنید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public GeneralEnums.ProviderEvidenceDocType? AttachmentTypeId { get; set; }

        [Display(Name = "نوع پیوست")]
        public string AttachmentTypeTitle { get; set; }

        [Search]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "فایل")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فایل مشخص نشده است")]
        public string RelativePath { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string CreatedAt { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public EditStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}