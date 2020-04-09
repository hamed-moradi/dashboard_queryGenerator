using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class ChangeLog : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        public int AdminId { get; set; }

        [InsertMandatoryField]
        public byte ActionType { get; set; }

        [InsertMandatoryField]
        public string Entity { get; set; }

        [InsertField]
        public long EntityId { get; set; }

        [InsertField]
        public string Data { get; set; }

        [InsertMandatoryField]
        public DateTime CreatedAt { get; set; }
    }

    public partial class ChangeLog
    {
        [HelperField]
        public long RowsCount { get; set; }

    }
}