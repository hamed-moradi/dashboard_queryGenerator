using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class PredictionViewModel : BaseViewModel
    {
        [Display(Name = "کاربر")]
        public int? UserId { get; set; }

        [Display(Name = "مسابقه")]
        public int? MatchId { get; set; }

        [Display(Name = "کلاب میزبان")]
        public int? HomeClubId { get; set; }

        [Display(Name = "امتیاز کلاب میزبان")]
        public int? HomeClubScore { get; set; }

        [Display(Name = "کلاب مهمان")]
        public int? AwayClubId { get; set; }

        [Display(Name = "امیتاز کلاب مهمان")]
        public int? AwayClubScore { get; set; }

        [Display(Name = "")]
        public byte? Series { get; set; }

        [Display(Name = "")]
        public int? Sets { get; set; }

        [Display(Name = "")]
        public int? ClubMemberId { get; set; }


        [Display(Name = "تاریخ پیش بینی")]
        public string CreatedAt { get; set; }

        [Display(Name = "مسابقه")]
        public string MatchImage { get; set; }

        [Display(Name = "کلاب میزبان")]
        public string HomeClubName { get; set; }

        [Display(Name = "کلاب مهمان")]
        public string AwayClubName { get; set; }


        [Display(Name = "کاربر")]
        //[Search]
        public string UserName { get; set; }

        [Display(Name = "مسابقه")]
        [Search]
        public string MatchTitle { get; set; }

        [Display(Name = "رویداد")]
        [Search]
        public int? EventId { get; set; }


        [Display(Name = "رویداد")]
        [Search]
        public string EventTitle { get; set; }


        [Display(Name = "امتیاز")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.String)]
        public int? UserPoint { get; set; }


        [Display(Name = "از تاریخ")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.DateTime)]
        public string FromDate { get; set; }

        [Display(Name = "تا تاریخ")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.DateTime)]
        public string ToDate { get; set; }
    }

    public class PredictionStatisticsViewModel
    {
        public int PredictionCount { get; set; }
        public int TotalPoints { get; set; }
    }
}