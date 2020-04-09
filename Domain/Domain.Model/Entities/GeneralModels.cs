using System.Collections.Generic;
using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class DropDownItemModel : IEntity
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
    }

    public partial class DropDownItemModel
    {
        [HelperField]
        public long RowsCount { get; set; }
    }
}