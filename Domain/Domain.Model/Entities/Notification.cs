using System;
using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class Notification : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        public string Body { get; set; }

        [InsertMandatoryField]
        public long? UserCount { get; set; }

        [InsertMandatoryField]
        public int? CreatorId { get; set; }

        [InsertField]
        public DateTime? CreatedAt { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? Status { get; set; }
    }

    public partial class Notification
    {
        [RelatedField(nameof(Admin), nameof(Admin.Username), nameof(CreatorId))]
        public string CreatorName { get; set; }

        [HelperField]
        public long RowsCount { get; set; }
    }
}