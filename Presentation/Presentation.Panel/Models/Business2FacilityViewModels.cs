using Asset.Infrastructure.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class Business2FacilityViewModel : BaseViewModel
    {
        [Display(Name = "کسب و کار")]
        public int? BusinessId { get; set; }

        [Display(Name = "کسب و کار")]
        public string BusinessTitle { get; set; }

        [Display(Name = "ویژگی")]
        public int? FacilityId { get; set; }

        [Search]
        [Display(Name = "ویژگی")]
        public string FacilityTitle { get; set; }

        [Display(Name = "مقدار")]
        public string Value { get; set; }

        [Display(Name = "اولویت")]
        public byte? Priority { get; set; }
        public long? ActivityId { get; set; }
        //public GeneralEnums.ActionType? ActionTypeId { get; set; }
        //public string CreatedAt { get; set; }
    }
    public class Business2FacilityHistoryViewModel
    {
        public List<Business2FacilityViewModel> Items { get; set; }
        public GeneralEnums.ActionType? ActionTypeId { get; set; }
        public string CreatedAt { get; set; }
        public int? CreatorId { get; set; }
        public string CreatorName { get; set; }
    }
}