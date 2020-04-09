using Domain.Model._App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Entities
{
    public partial class ReceivedSms : IBaseEntity
    {
        public long? Id { get; set; }
        public string MSISDN { get; set; }
        public string Body { get; set; }
        public string ShortCode { get; set; }
        public string SenderIp { get; set; }
        public long? WalletTransactionId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? ReceivedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public byte? Status { get; set; }
        public int? CorrelatorId { get; set; }
        public Guid? TaskLogGroupId { get; set; }
    }
    public partial class ReceivedSms
    {
        [HelperField]
        public long RowsCount { get; set; }
    }
}
