using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class UserLeaderBoardViewModel : BaseViewModel
    {
        [Search]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Display(Name = "شناسه رویداد")]
        public int? EventId { get; set; }

        [Display(Name = "عنوان رویداد")]
        public string EventTitle { get; set; }

        [Display(Name = "مجموع امتیازات")]
        public int TotalPoints { get; set; }



        [Display(Name = "پیشبینی درست نتیجه بازی و تعداد گل ها")]
        public int CorrectResultAndNumberOfGoals { get; set; }

        [Display(Name = "پیشبینی درست تفاضل گل")]
        public int GoalDifference { get; set; }

        [Display(Name = "پیشبینی درست برنده و بازنده")]
        public int CorrectCinnerAndLoser { get; set; }

        [Display(Name = "پیشبینی درست تعداد گل یک تیم")]
        public int CorrectNumberOfGoalsATeam { get; set; }

        [Display(Name = "پیشبینی اشتباه")]
        public int WrongPrediction { get; set; }
    }
}