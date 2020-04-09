using System.ComponentModel.DataAnnotations;

namespace Presentation.Panel.Models
{
    public class ServiceRequestPreInvoiceItemViewModel:BaseViewModel
    {
        public int? PrefactorId { get; set; }
        public long? ServiceRequestItem2BusinessId { get; set; }
        [Display(Name ="هزینه")]
        public decimal? Price { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "وضعیت")]
        public byte? Status { get; set; }
        public string LastModifiedAt { get; set; }
        public string CreatedAt { get; set; }
        [Display(Name = "نام سرویس")]
        public string ServiceName { get; set; }
    }
}