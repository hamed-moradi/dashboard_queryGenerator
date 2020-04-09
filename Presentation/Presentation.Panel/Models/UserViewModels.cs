using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class UserViewModel : BaseViewModel
    {
        [Search]
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "{0} را وارد نمایید")]
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

        [Search]
        [Display(Name = "تلفن")]
        [RegularExpression("((^|, )(0?[1-9][0-9]{9}|\u0660[\u0661-\u0669][\u0660-\u0669]{9}|[\u06F1-\u06F9][\u06F0-\u06F9]{9}))+$", ErrorMessage = "شماره تلفن نامعتبر است")]
        public string Phone { get; set; }

        [Display(Name = "تلفن فعال شده ؟")]
        public bool? PhoneVerified { get; set; }

        [Display(Name = "کد فعالسازی")]
        public string ActivitionCode { get; set; }

        [Search]
        [Display(Name = "ایمیل")]
        [RegularExpression("^((([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+(\\.([a-z]|\\d|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])+)*)|((\\x22)((((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(([\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x7f]|\\x21|[\\x23-\\x5b]|[\\x5d-\\x7e]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(\\\\([\\x01-\\x09\\x0b\\x0c\\x0d-\\x7f]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF]))))*(((\\x20|\\x09)*(\\x0d\\x0a))?(\\x20|\\x09)+)?(\\x22)))@((([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|\\d|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.)+(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])|(([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])([a-z]|\\d|-|\\.|_|~|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])*([a-z]|[\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF])))\\.?$", ErrorMessage = "آدرس ایمیل نامعتبر است")]
        public string Email { get; set; }

        [Display(Name = "ایمیل فعال شده ؟")]
        public bool? EmailVerified { get; set; }

        [Search]
        [Display(Name = "نام مستعار")]
        public string NickName { get; set; }

        [Display(Name = "عضویت از")]
        public GeneralEnums.IdentityProvider? IdentityProvider { get; set; }

        [Display(Name = "عضویت از")]
        public string IdentityProviderTitle { get; set; }

        [Display(Name = "آواتار")]
        public string Avatar { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.Enum)]
        public new ClubStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }

        [Display(Name = "تعداد پیش بینی ها")]
        public int NumberOfPredictions { get; set; }

        [Display(Name = "مجموع امتیازات")]
        public int TotalPoints { get; set; }
    }
}