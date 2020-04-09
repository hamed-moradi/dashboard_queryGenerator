using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class Role2Module : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        public int? RoleId { get; set; }

        [InsertMandatoryField]
        public int? ModuleId { get; set; }
    }

    public partial class Role2Module
    {
        [HelperField]
        public virtual Module Module { get; set; }

        [HelperField]
        public virtual Role Role { get; set; }

        [HelperField]
        public long RowsCount { get; set; }
    }
}