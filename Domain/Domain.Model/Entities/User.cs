using Domain.Model._App;
using Newtonsoft.Json;
using System;

namespace Domain.Model.Entities
{
    public partial class User : SimpleEntity
    {
        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string UserName { get; set; }

        [JsonIgnore]
        [InsertField]
        [UpdateField]
        public string Password { get; set; }

        [InsertField]
        [UpdateField]
        public string ActivitionCode { get; set; }

        [InsertField]
        [UpdateField]
        public string NickName { get; set; }

        [InsertField]
        [UpdateField]
        public string Email { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public bool? EmailVerified { get; set; }

        [InsertField]
        [UpdateField]
        public string Phone { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public bool? PhoneVerified { get; set; }

        [InsertField]
        [UpdateField]
        public string Avatar { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? IdentityProvider { get; set; }
    }

    public partial class User
    {
        [HelperField]
        public int PendingEvidenceCount { get; set; }

        //[HelperField]
        //public string FullName
        //{
        //    get
        //    {
        //        var name = ($"{FirstName} {LastName}").Trim();
        //        if (string.IsNullOrWhiteSpace(name)) name = CellPhone;
        //        if (string.IsNullOrWhiteSpace(name)) name = Email;
        //        return name;
        //    }
        //}

        //[RelatedField(nameof(Admin), nameof(Admin.Username), nameof(CreatorId))]
        //public override string CreatorName { get; set; }

        [HelperField]
        public long[] ActivityId { get; set; }

        public int NumberOfPredictions { get; set; }
        public int TotalPoints { get; set; }
    }
}