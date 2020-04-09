using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class Match2ClubViewModel : BaseViewModel
    {
        [Display(Name = "رویداد")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? EventId { get; set; }

        [Display(Name = "مسابقه")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? MatchId { get; set; }

        [Display(Name = "کلاب میزبان")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? HomeClubId { get; set; }

        [Display(Name = "امتیاز کلاب میزبان")]
        public int? HomeClubScore { get; set; }

        [Display(Name = "کلاب میهمان")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? AwayClubId { get; set; }

        [Display(Name = "امتیاز کلاب میهمان")]
        public int? AwayClubScore { get; set; }



        [Search]
        [Display(Name = "مسابقه")]
        public string MatchTitle { get; set; }

        [Display(Name = "تاریخ برگزاری")]
        public DateTime OccurrenceDate { get; set; }


        [Display(Name = "تصویر مسابقه")]
        public string MatchImage { get; set; }

        [Search]
        [Display(Name = "کلاب میزبان")]
        public string HomeClubName { get; set; }

        [Search]
        [Display(Name = "کلاب میهمان")]
        public string AwayClubName { get; set; }
    }
}