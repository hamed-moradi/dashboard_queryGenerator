using System.Collections.Generic;
using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class Role : SimpleEntity
    {
        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Title { get; set; }

        [InsertField]
        [UpdateField]
        public string Description { get; set; }

        [InsertField]
        [UpdateField]
        public int? ParentId { get; set; }
    }

    public partial class Role
    {
        [RelatedField(nameof(Role), nameof(Role.Title), nameof(Role) + "].[" + nameof(ParentId), false)]
        public virtual string ParentTitle { get; set; }

        [HelperField]
        public virtual IEnumerable<Role2Module> Role2Modules { get; set; }
    }
}