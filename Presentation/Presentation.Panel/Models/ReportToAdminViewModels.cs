using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class ReportToAdminViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان خالی است")]
        public string Title { get; set; }

        [Search]
        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "اولویت")]
        [Required(ErrorMessage = "فیلد اولویت خالی است")]
        public byte? Priority { get; set; }

        [Display(Name = "ریپورت")]
        public long? ObjectId { get; set; }

        [Display(Name = "ریپورت")]
        public string ObjectName { get; set; }

        [Display(Name = "نوع")]
        public byte? ObjectTypeId { get; set; }

        [Display(Name = "خوانده شده در")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string ReadAt { get; set; }
    }
}