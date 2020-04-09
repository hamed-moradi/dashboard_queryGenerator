namespace Asset.Infrastructure.Common
{
    public class ClientDeviceInfo //: IValidatableObject
    {
        public string Token { get; set; }
        public string IP { get; set; }
        public string DeviceId { get; set; }
        public string OS { get; set; }
        public string DeviceName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public GeoLocation UserLocation { get; set; }
    }
}
