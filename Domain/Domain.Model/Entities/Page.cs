using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class Page : Entity
    {
        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string UniqueName { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Title { get; set; }

        [InsertField]
        [UpdateField]
        public string SubTitle { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Summary { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Body { get; set; }

        [InsertField]
        [UpdateField]
        public string Thumbnail { get; set; }

        [InsertField]
        [UpdateField]
        public string Photo { get; set; }
    }

    public partial class Page
    {
    }
}