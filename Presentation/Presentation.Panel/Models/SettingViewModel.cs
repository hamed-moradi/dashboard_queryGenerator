using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Presentation.Panel.Models
{
    public class SettingViewModel : BaseViewModel
    {
        [Display(Name = "کلید")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد کلید خالی است")]
        public string Key { get; set; }

        [Display(Name = "نام")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد نام خالی است")]
        public string Name { get; set; }

        [Display(Name = "مقدار")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد مقدار خالی است")]
        public string Value { get; set; }
    }
}