using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public partial class ClubMember : SimpleEntity
    {
        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string FullName { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte Age { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte Gender { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte Rank { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int ClubId { get; set; }
    }
}