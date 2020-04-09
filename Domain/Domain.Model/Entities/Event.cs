using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class Event : SimpleEntity
    {
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
        public DateTime? StartedAt { get; set; }

        [InsertField]
        [UpdateField]
        public DateTime? EndedAt { get; set; }
    }
}