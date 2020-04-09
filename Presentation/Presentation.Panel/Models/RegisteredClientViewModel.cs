using System.ComponentModel.DataAnnotations;
using Asset.Infrastructure.Common;

namespace Presentation.Panel.Models
{
    public class RegisteredClientViewModel
    {
        [Display(Name = "شناسه")]
        public int? Id { get; set; }

        [Display(Name = "نام کلید")]
        public string UniqueKey { get; set; }

        [Display(Name = "عنوان")]
        public string Name { get; set; }

        [Display(Name = "کلید امنیتی")]
        public string Secret { get; set; }
        
        public int? ClientTypeId { get; set; }
        [Display(Name = "نوع")]
        public string ClientTypeName { get; set; }

        [Display(Name = "اوریجین مجاز")]
        public string AllowedOrigin { get; set; }

        [Display(Name = "وضعیت")]
        public byte? IsActive { get; set; }
        public string IsActiveName { get; set; }

        [Display(Name = "تصویر پروفایل")]
        public string Avatar { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "آخرین نسخه")]
        public string LastVersion { get; set; }
        public int? LastStableVersion { get; set; }

    }


}
