using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class Match2Club : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? MatchId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? HomeClubId { get; set; }

        [InsertField]
        [UpdateField]
        public int? HomeClubScore { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? AwayClubId { get; set; }

        [InsertField]
        [UpdateField]
        public int? AwayClubScore { get; set; }

        [InsertField]
        [UpdateField]
        public int? ClubMemberId { get; set; }
    }

    public partial class Match2Club
    {
        [HelperField]
        public long RowsCount { get; set; }

        [RelatedField(nameof(Match), nameof(Match.Title), nameof(Match2Club.MatchId), true)]
        public string MatchTitle { get; set; }

        [RelatedField(nameof(Match), nameof(Match.OccurrenceDate), nameof(Match2Club.MatchId), true)]
        public DateTime? OccurrenceDate { get; set; }

        [RelatedField(nameof(Match), nameof(Match.Image), nameof(Match2Club.MatchId), true)]
        public string MatchImage { get; set; }

        [RelatedField(nameof(Club), nameof(Club.Name), nameof(Match2Club.HomeClubId), true)]
        public string HomeClubName { get; set; }

        [RelatedField(nameof(Club), nameof(Club.Name), nameof(Match2Club.AwayClubId), true)]
        public string AwayClubName { get; set; }

        [RelatedField(nameof(Match), nameof(Match.EventId), nameof(Match2Club.MatchId), true)]
        public int? EventId { get; set; }
    }
}