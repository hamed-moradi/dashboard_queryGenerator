using System;
using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class Admin : SimpleEntity
    {
        [InsertMandatoryField]
        public string Username { get; set; }

        [InsertMandatoryField]
        public string Password { get; set; }

        [InsertField]
        [UpdateField]
        public string FirstName { get; set; }

        [InsertField]
        [UpdateField]
        public string LastName { get; set; }

        [InsertMandatoryField]
        public string Phone { get; set; }

        [InsertField]
        [UpdateField]
        public string Email { get; set; }

        [InsertField]
        [UpdateField]
        public byte? Gender { get; set; }

        [InsertField]
        [UpdateField]
        public string Avatar { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? RoleId { get; set; }

        [UpdateField]        
        public DateTime? LastLogin { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? Status { get; set; }
    }

    public partial class Admin
    {
        [HelperField]
        public string FullName
        {
            get
            {
                var name = (FirstName + " " + LastName).Trim();
                if (name == "") name = Username;
                return name;
            }
        }
        [RelatedField(nameof(Role), nameof(Role.Title), nameof(Admin.RoleId), true)]
        public string RoleTitle { get; set; }
    }
}