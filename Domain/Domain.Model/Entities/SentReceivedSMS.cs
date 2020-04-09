using Domain.Model._App;
using System;

namespace Domain.Model.Entities
{
    public class SentReceivedSMS:IBaseEntity
    {
        [HelperField]
        public string Body { get; set; }
        [HelperField]
        public DateTime? CreatedAt { get; set; }
        [HelperField]
        public bool? IsSent { get; set; }
        [HelperField]
        public int? RowNumber { get; set; }
        [HelperField]
        public string MSISDN { get; set; }
        [HelperField]
        public long RowsCount { get; set; }
    }
}
