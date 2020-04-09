using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class Club : SimpleEntity
    {
        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? EventId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Name { get; set; }

        [InsertField]
        [UpdateField]
        public string Abbreviation { get; set; }

        [InsertField]
        [UpdateField]
        public string Description { get; set; }

        [InsertField]
        [UpdateField]
        public string Thumbnail { get; set; }

        [InsertField]
        [UpdateField]
        public string Image { get; set; }
    }

    public partial class Club
    {
        [RelatedField(nameof(Event), nameof(Event.Title), nameof(Club.EventId), true)]
        public string EventTitle { get; set; }
    }
}