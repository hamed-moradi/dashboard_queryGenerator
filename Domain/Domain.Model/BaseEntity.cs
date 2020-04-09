using System;
using Domain.Model.Entities;
using Domain.Model._App;
using Newtonsoft.Json;

namespace Domain.Model
{
    public interface IBaseEntity
    {
        [JsonIgnore]
        long RowsCount { get; set; }
    }

    public interface IEntity : IBaseEntity
    {
        int? Id { get; set; }
    }

    public interface KeyEntity : IBaseEntity
    {
        string Key { get; set; }
    }

    public class SimpleEntity : IBaseEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? Status { get; set; }

        [HelperField]
        public long RowsCount { get; set; }
    }

    public partial class Entity : IEntity
    {
        public int? Id { get; set; }

        [InsertMandatoryField]
        public int? CreatorId { get; set; }

        [InsertField]
        public DateTime? CreatedAt { get; set; }

        [UpdateMandatoryField]
        public int? UpdaterId { get; set; }

        [UpdateField]
        public DateTime? UpdatedAt { get; set; }

        [InsertMandatoryField]
        [UpdateMandatoryField]
        public byte? Status { get; set; }
    }

    public partial class Entity
    {
        [RelatedField(nameof(Admin), nameof(Admin.Username), nameof(CreatorId), true)]
        public virtual string CreatorName { get; set; }

        [RelatedField(nameof(Admin), nameof(Admin.Username), nameof(UpdaterId))]
        public virtual string UpdaterName { get; set; }

        [HelperField]
        public long RowsCount { get; set; }
    }
}