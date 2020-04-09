using Domain.Model._App;

namespace Domain.Model.Entities
{

    public partial class AppSetting : KeyEntity
    {
        public string Key { get; set; }
        
        [UpdateMandatoryField]
        public string Title { get; set; }
        
        [UpdateMandatoryField]
        public string Value { get; set; }
        
        [UpdateField]
        public string Description { get; set; }
        
        [UpdateField]
        public string Icon { get; set; }

        public int? Priority { get; set; }
        
    }
    public partial class AppSetting
    {
        [HelperField]
        public long RowsCount { get; set; }
    }

}
