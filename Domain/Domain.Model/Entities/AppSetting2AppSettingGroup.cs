using Domain.Model._App;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Domain.Model.Entities
{
    public partial class AppSetting2AppSettingGroup : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string AppSettingKey { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public int? AppSettingGroupId { get; set; }
    }

    public partial class AppSetting2AppSettingGroup
    {
        [HelperField]
        public long RowsCount { get; set; }
        

        [RelatedField(nameof(AppSettingGroup), nameof(AppSettingGroup.Title), nameof(AppSettingGroupId), true)]
        public string Title { get; set; }

        [RelatedField(nameof(AppSettingGroup), nameof(AppSettingGroup.Icon), nameof(AppSettingGroupId), true)]
        public string Icon { get; set; }

        [RelatedField(nameof(AppSettingGroup), nameof(AppSettingGroup.ParentId), nameof(AppSettingGroupId), true)]
        public int? ParentId { get; set; }
    }
    
}
