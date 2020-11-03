using Dapper.Contrib.Extensions;
using System;

namespace Pushenger.Core.Entities
{
    [Table("notificationdetail")]
    public class NotificationDetail :BaseEntity
    {
        public int NotificationId { get; set; }
        public int SubscriptionId { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
