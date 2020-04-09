using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Model.Entities;
using MD.PersianDateTime;

namespace Presentation.Panel.Models
{
    public class PermissionViewModel
    {
        [Display(Name = "نقض")]
        public int? RoleId { get; set; }

        [Display(Name = "ماژول")]
        public int? ModuleId { get; set; }
    }
}