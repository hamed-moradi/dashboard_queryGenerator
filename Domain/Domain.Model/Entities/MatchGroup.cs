using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class MatchGroup : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Title { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? EventId { get; set; }

        [InsertField]
        [UpdateField]
        public string Description { get; set; }
    }

    public partial class MatchGroup
    {
        [RelatedField(nameof(Event), nameof(Event.Title), nameof(Match.EventId), true)]
        public string EventTitle { get; set; }

        [HelperField]
        public long RowsCount { get; set; }
    }
}