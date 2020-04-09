using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Model.Entities;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class VehicleModelViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "کارخانه")]
        [Required(ErrorMessage = "کارخانه را وارد نمایید")]
        public int? ManufactureId { get; set; }

        [Search]
        [Display(Name = "کارخانه")]
        public string ManufactureTitle { get; set; }

        [Search]
        [Display(Name = "سال شروع تولید")]
        [Required(ErrorMessage = "سال شروع تولید را وارد نمایید")]
        public int? BirthYear { get; set; }

        [Search]
        [Display(Name = "سال پایان تولید")]
        [Required(ErrorMessage = "سال پایان تولید را وارد نمایید")]
        public int? DeathYear { get; set; }

        [Display(Name = "نوع تاریخ")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        [Required(ErrorMessage = "نوع تاریخ را وارد نمایید")]
        public GeneralEnums.DateType? DateTypeId { get; set; }

        [Display(Name = "نوع تاریخ")]
        public string DateTypeTitle { get; set; }
    }
}