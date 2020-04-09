using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Model.Entities;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class VehicleTrimViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "مدل")]
        [Required(ErrorMessage = "مدل را وارد نمایید")]
        public int? ModelId { get; set; }

        [Search]
        [Display(Name = "مدل")]
        public string ModelTitle { get; set; }

        [Search]
        [Display(Name = "سال شروع تولید")]
        public int? BirthYear { get; set; }

        [Search]
        [Display(Name = "سال پایان تولید")]
        public int? DeathYear { get; set; }

        [Display(Name = "نوع تاریخ")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public GeneralEnums.DateType? DateTypeId { get; set; }

        [Display(Name = "نوع تاریخ")]
        public string DateTypeTitle { get; set; }
    }
}