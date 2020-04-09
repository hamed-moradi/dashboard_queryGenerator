using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class Match : SimpleEntity
    {
        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? EventId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? GroupId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Title { get; set; }

        [InsertField]
        [UpdateField]
        public string Description { get; set; }

        [InsertField]
        [UpdateField]
        public string Thumbnail { get; set; }

        [InsertField]
        [UpdateField]
        public string Image { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? Priority { get; set; }

        [InsertField]
        [UpdateField]
        public DateTime? OccurrenceDate { get; set; }

        [InsertField]
        [UpdateField]
        public DateTime? PredictionDeadline { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? PredictionWeight { get; set; }        
    }

    public partial class Match
    {
        [RelatedField(nameof(Event), nameof(Event.Title), nameof(Match.EventId), true)]
        public string EventTitle { get; set; }

        [RelatedField(nameof(MatchGroup), nameof(MatchGroup.Title), nameof(Match.GroupId), true)]
        public string GroupTitle { get; set; }


        public int? HomeClubId { get; set; }
        public int? AwayClubId { get; set; }
    }
}