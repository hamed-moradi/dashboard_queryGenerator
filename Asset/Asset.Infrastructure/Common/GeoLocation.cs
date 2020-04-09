using System.ComponentModel.DataAnnotations;

namespace Asset.Infrastructure.Common
{
    public class GeoLocation
    {
        [Required(ErrorMessage = "لطفا عرض جغرافیایی را مشخص کنید.")]
        [Range(-90.0, 90.0, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "GeoLatRangeExcitedMessage")]
        public double? Lat { get; set; }

        [Required(ErrorMessage = "لطفا طول جغرافیایی را مشخص کنید.")]
        [Range(-180.0, 180.0, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "GeoLongRangeExcitedMessage")]
        public double? Lng { get; set; }

        public override string ToString()
        {
            return $"lat:{Lat} | lng:{Lng}";
        }
    }
}
