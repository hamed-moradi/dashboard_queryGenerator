using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Model.Entities;
using MD.PersianDateTime;

namespace Presentation.Panel.Models
{
    public class RoleViewModel : ViewModel
    {
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد عنوان خالی است")]
        [Search]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        public IList<Role2Module> Role2Modules { get; set; }
    }
}