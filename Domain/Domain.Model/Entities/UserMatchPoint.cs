using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class UserMatchPoint : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? UserId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int EventId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int MatchId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int Point { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public DateTime CreatedAt { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int PredictionId { get; set; }        
    }

    public partial class UserMatchPoint
    {
        [HelperField]
        public long RowsCount { get; set; }
    }
}