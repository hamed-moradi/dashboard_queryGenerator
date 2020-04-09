using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class User2Notification : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? UserId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? NotificationId { get; set; }

        [InsertField]
        [UpdateField]
        public DateTime? DeliveredAt { get; set; }

        [InsertField]
        [UpdateField]
        public DateTime? SeenAt { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public DateTime? SentAt { get; set; }
    }

    public partial class User2Notification
    {
        //[RelatedField(nameof(User), nameof(User.CellPhone), nameof(UserId), true)]
        //public string UserName { get; set; }

        [HelperField]
        public long RowsCount { get; set; }
    }
}