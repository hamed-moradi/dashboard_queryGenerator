using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class Prediction : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? UserId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? MatchId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? HomeClubId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? HomeClubScore { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? AwayClubId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? AwayClubScore { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? Series { get; set; }

        [InsertField]
        [UpdateField]
        public int? Sets { get; set; }

        [InsertField]
        [UpdateField]
        public int? ClubMemberId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public DateTime? CreatedAt { get; set; }
    }

    public partial class Prediction
    {
        [HelperField]
        public long RowsCount { get; set; }

        [RelatedField(nameof(User), nameof(User.UserName), nameof(Prediction.UserId), true)]
        public string UserName { get; set; }

        [RelatedField(nameof(Match), nameof(Match.Title), nameof(Prediction.MatchId), true)]
        public string MatchTitle { get; set; }

        [RelatedField(nameof(Match), nameof(Match.Image), nameof(Prediction.MatchId), true)]
        public string MatchImage { get; set; }

        [RelatedField(nameof(Club), nameof(Club.Name), nameof(Prediction.HomeClubId), true)]
        public string HomeClubName { get; set; }

        [RelatedField(nameof(Club), nameof(Club.Name), nameof(Prediction.AwayClubId), true)]
        public string AwayClubName { get; set; }

        [RelatedField(nameof(Match), nameof(Match.EventId), nameof(Match2Club.MatchId), true)]
        public int? EventId { get; set; }

        public int? UserPoint { get; set; }


        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }







    public class PredictionStatistics : IBaseEntity
    {
        public int PredictionCount { get; set; }
        public int TotalPoints { get; set; }

        [HelperField]
        public long RowsCount { get; set; }
    }
}