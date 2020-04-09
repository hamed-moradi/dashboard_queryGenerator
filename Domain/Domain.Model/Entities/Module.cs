using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class Module : SimpleEntity
    {
        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Title { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Path { get; set; }

        [InsertField]
        [UpdateField]
        public string Description { get; set; }

        [InsertField]
        [UpdateField]
        public string Icon { get; set; }

        [InsertField]
        [UpdateField]
        public int? ParentId { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string HttpMethod { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? Category { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? Priority { get; set; }
    }

    public partial class Module
    {
        [RelatedField(nameof(Module), nameof(Module.Title), nameof(Module) + "].[" + nameof(ParentId), false)]
        public string ParentTitle { get; set; }
    }
}