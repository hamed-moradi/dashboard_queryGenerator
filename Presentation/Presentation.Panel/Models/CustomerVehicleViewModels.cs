using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class CustomerVehicleViewModel : BaseViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "مشتری")]
        [Required(ErrorMessage = "مشتری را وارد نمایید")]
        public int? CustomerId { get; set; }

        [Display(Name = "مشتری")]
        public string CustomerName { get; set; }

        [Display(Name = "مدل")]
        public int? ModelId { get; set; }

        [Display(Name = "مدل")]
        public string ModelTitle { get; set; }

        [Display(Name = "ویژگی")]
        public int? TrimId { get; set; }

        [Display(Name = "ویژگی")]
        public string TrimTitle { get; set; }

        [Display(Name = "تاریخ تولید")]
        public int? ProductionDate { get; set; }

        [Display(Name = "کارکرد")]
        [Required(ErrorMessage = "کارکرد را وارد نمایید")]
        public int? Mileage { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public EditStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
    }
}