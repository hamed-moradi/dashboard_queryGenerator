using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class MatchViewModel : BaseViewModel
    {
        [Search]
        [Display(Name = "عنوان")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "فیلد {0} خالی است")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "نمایه")]
        public string Thumbnail { get; set; }

        [Display(Name = "تصویر")]
        public string Image { get; set; }

        [Display(Name = "اولویت")]
        public int? Priority { get; set; }

        [Display(Name = "تاریخ برگزاری")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.DateTime)]
        public string OccurrenceDate { get; set; }

        [Display(Name = "زمان اتمام پیشبینی")]
        public string PredictionDeadline { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        public new MatchesStatus? Status { get; set; }

        [Display(Name = "وضعیت")]
        [Search(Asset.Infrastructure.Common.GeneralEnums.SearchFieldType.Enum)]
        public new MatchesStatusForIndex? StatusForIndex { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }

        [Display(Name = "وضعیت")]
        public string StatusTitleForIndex { get; set; }

        [Display(Name = "رویداد")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? EventId { get; set; }

        [Display(Name = "گروه")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? GroupId { get; set; }

        [Display(Name = "کلاب میزبان")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? HomeClubId { get; set; }

        [Display(Name = "کلاب میهمان")]
        [Required(ErrorMessage = "{0} را انتخاب نمایید")]
        public int? AwayClubId { get; set; }

        [Display(Name = "ارزش مسابقه")]
        public int? PredictionWeight { get; set; }

        [Display(Name = "رویداد")]
        [Search]
        public string EventTitle { get; set; }

        [Display(Name = "گروه")]
        [Search]
        public string GroupTitle { get; set; }

        [Display(Name = "امتیاز کلاب میزبان")]
        public int? HomeClubScore { get; set; }

        [Display(Name = "امتیاز کلاب مهمان")]
        public int? AwayClubScore { get; set; }

        [Display(Name = "کلاب میزبان")]
        public string HomeClubName { get; set; }

        [Display(Name = "کلاب مهمان")]
        public string AwayClubName { get; set; }
    }
}