using Asset.Infrastructure.Common;
using System.ComponentModel.DataAnnotations;


namespace Presentation.Panel.Models
{
    public class ProviderViewModel : UserViewModel
    {
        
        public int? ActiveProviderCount { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "وضعیت را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public new UserEditStatus? Status { get; set; }

        [Display(Name = "تغییر یافته")]
        public bool? HasChanged { get; set; }

        [Display(Name = "وضعیت مدارک")]
        //[Required(ErrorMessage = "وضعیت مدارک را وارد نمایید")]
        [Search(GeneralEnums.SearchFieldType.Enum)]
        public GeneralEnums.EvidenceStatusType? EvidenceStatusId { get; set; }

        [Display(Name = "وضعیت مدارک")]
        public string EvidenceStatusTitle { get; set; }
        public long[] ActivityId { get; set; }
        [Display(Name = "تعداد مدارک معلق")]
        public int? PendingEvidenceCount { get; set; }
        public GeneralEnums.ActionType? ActionTypeId { get; set; }
    }
}