using System;
using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class Session : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        public string Token { get; set; }

        [InsertMandatoryField]
        public string IP { get; set; }

        [InsertMandatoryField]
        public int? UserId { get; set; }

        [InsertMandatoryField]
        public int? DeviceId { get; set; }

        [InsertField]
        public string OS { get; set; }

        [InsertField]
        public string Version { get; set; }

        [InsertField]
        public string DeviceName { get; set; }

        [InsertField]
        public string Browser { get; set; }

        [InsertMandatoryField]
        public byte? Status { get; set; }

        [InsertField]
        public string FcmId { get; set; }
    }

    public partial class Session
    {
        [HelperField]
        public long RowsCount { get; set; }
    }
}