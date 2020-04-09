using System.Collections.Generic;

namespace Presentation.Panel.Models
{
    public class CreatePreInvoiceViewModel
    {
        public int BusinessId { get; set; }
        public string BusinessTitle { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Description { get; set; }
        public int ServiceRequestId { get; set; }
        public long ServiceRequest2BusinessId { get; set; }
        public string DueDate { get; set; }
        public string DueTime { get; set; }
        public decimal Fee { get; set; }
        public List<PreInvoiceItemViewModel> InvoiceItems { get; set; }
    }
    public class PreInvoiceItemViewModel
    {
        public long FacilityRequest2BusinessItemId { get; set; }
        public decimal Price { get; set; }
    }
}