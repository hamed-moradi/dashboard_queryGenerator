using System.ComponentModel.DataAnnotations;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class AdminViewModel : ViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری را وارد نمایید")]
        [Search]
        public string UserName { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "گذروازه را وارد نمایید")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "حداقل 6 کاراکتر")]
        public string Password { get; set; }

        [Display(Name = "تکرار گذرواژه")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "عدم همخوانی گذرواژه ها")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "نام")]
        [Search]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Search]
        public string LastName { get; set; }

        [Display(Name = "نام")]
        public string FullName { get; set; }

        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "شماره تلفن را وارد نمایید")]
        [Search]
        public string Phone { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "ایمیل را وارد نمایید")]
        [Search]
        public string Email { get; set; }

        [Display(Name = "جنسیت")]
        public GeneralEnums.Gender? Gender { get; set; }

        [Display(Name = "جنسیت")]
        public string GenderTitle { get; set; }

        public int? GeoId { get; set; }

        [Display(Name = "آواتار")]
        public string Avatar { get; set; }

        [Display(Name = "نقش")]
        [Required(ErrorMessage = "نقش را انتخاب نمایید")]
        public int? RoleId { get; set; }

        [Display(Name = "نقش")]
        public string RoleTitle { get; set; }

        [Display(Name = "تاریخ آخرین ورود")]
        [Search(GeneralEnums.SearchFieldType.DateTime)]
        public string LastLogin { get; set; }

    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "شناسه یافت نشد")]
        public int? Id { get; set; }

        [Display(Name = "گذرواژه جاری")]
        [Required(ErrorMessage = "گذروازه جاری خود را وارد نمایید")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "گذروازه جدید را وارد نمایید")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "حداقل 6 کاراکتر")]
        public string Password { get; set; }

        [Display(Name = "تکرار گذرواژه")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "عدم همخوانی گذرواژه ها")]
        public string ConfirmPassword { get; set; }
    }
}