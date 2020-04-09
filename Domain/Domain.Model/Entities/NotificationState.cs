using Domain.Model._App;

namespace Domain.Model.Entities
{
    public partial class NotificationState : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public string Title { get; set; }
    }

    public partial class NotificationState
    {
        [HelperField]
        public long RowsCount { get; set; }
    }
}