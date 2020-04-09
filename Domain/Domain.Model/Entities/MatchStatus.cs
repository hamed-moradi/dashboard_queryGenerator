using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class MatchStatus : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Title { get; set; }

        [InsertField]
        [UpdateField]
        public string Description { get; set; }
    }
    public partial class MatchStatus
    {
        [HelperField]
        public long RowsCount { get; set; }
    }
}