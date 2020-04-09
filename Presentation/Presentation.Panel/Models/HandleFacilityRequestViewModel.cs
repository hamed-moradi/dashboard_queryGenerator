namespace Presentation.Panel.Models
{
    public class HandleFacilityRequestViewModel
    {
        public int BusinessId { get; set; }
        public string BusinessTitle { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public int ServiceRequestId { get; set; }
        public long ServiceRequest2BusinessId { get; set; }
    }
}