using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Exception")]
    public partial class Exceptions : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public DateTime CreatedAt { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string URL { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Data { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Message { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Source { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string StackTrace { get; set; }

        [InsertField]
        [UpdateField]
        public string TargetSite { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string IP { get; set; }
    }

    public partial class Exceptions
    {
        [HelperField]
        public long RowsCount { get; set; }
    }

}