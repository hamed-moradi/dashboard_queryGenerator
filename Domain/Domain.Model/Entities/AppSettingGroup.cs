using System;
using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class AppSettingGroup : IEntity
    {
        public int? Id { get; set; }

        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Icon { get; set; }
        
        public int? ParentId { get; set; }
    }

    public partial class AppSettingGroup
    {
        [HelperField]
        public long RowsCount { get; set; }
        //[RelatedField(nameof(Category), nameof(Title), nameof(ParentId))]
        //public string ParentTitle { get; set; }
    }
}