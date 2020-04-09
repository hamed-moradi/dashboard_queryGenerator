
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Domain.Model.Entities;

namespace Presentation.Panel.Models
{
    public class VehicleManufactureViewModel : ViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "عنوان را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Search]
        [Display(Name = "وب سایت")]
        public string Website { get; set; }

        [Display(Name = "اواتار")]
        public string Avatar { get; set; }
    }
}