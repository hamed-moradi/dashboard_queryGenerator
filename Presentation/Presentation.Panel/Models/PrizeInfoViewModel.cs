using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class PrizeInfoViewModel
    {
        [Display(Name = "شناسه")]
        public int? Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "مرحله")]
        public int? StepNo { get; set; }
        [Display(Name = "دعوتهای لازم برای کسب جایزه")]
        public int? UserCountCondition { get; set; }
        [Display(Name = "معادل ریالی")]
        public int? EquivalentRial { get; set; }
        [Display(Name = "جایزه")]
        public string Award { get; set; }
        public int? CheckoutId { get; set; }
        [Display(Name = "تعداد درخواست کاربر")]
        public int? ViewCount { get; set; }
        [Display(Name = "تعداد ارسال از پنل")]
        public int? UserSentCount { get; set; }
        public int? Filter { get; set; }
        [Display(Name = "تاریخ")]
        public string CheckoutDate { get; set; }
    }
}