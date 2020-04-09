using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class PrizeStatisticViewModel
    {
        [Display(Name = "تعداد دعوت های موفق فعلی	")]
        public int CurrentInvite { get; set; }
        [Display(Name = "تعداد دعوت های تکراری	")]
        public int RepeatedInvite { get; set; }
        [Display(Name = "تعداد دعوت های انصراف داده شده کمتر از 6 ساعت	")]
        public int InvalidInvite { get; set; }
        [Display(Name = "تعداد دعوت های محاسبه شده	")]
        public int CalculatedInvite { get; set; }
        [Display(Name = "تعداد دعوت های کل	")]
        public int TotalInvite { get; set; }
        [Display(Name = "تعداد دعوت های غیرقابل شارژ	")]
        public int WithoutPaid { get; set; }
    }
}