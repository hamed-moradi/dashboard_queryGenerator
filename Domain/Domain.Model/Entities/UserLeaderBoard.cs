using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class UserLeaderBoard : IEntity
    {
        public int? Id { get; set; }

        public string UserName { get; set; }

        public int? EventId { get; set; }
        public string EventTitle { get; set; }

        public int? TotalPoints { get; set; }

        public int CorrectResultAndNumberOfGoals { get; set; }

        public int GoalDifference { get; set; }

        public int CorrectCinnerAndLoser { get; set; }

        public int CorrectNumberOfGoalsATeam { get; set; }

        public int WrongPrediction { get; set; }

    }

    public partial class UserLeaderBoard
    {
        [HelperField]
        public long RowsCount { get; set; }
    }
}