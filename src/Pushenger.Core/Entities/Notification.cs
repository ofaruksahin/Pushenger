using Dapper.Contrib.Extensions;
using System;

namespace Pushenger.Core.Entities
{
    [Table("notification")]
    public class Notification : BaseEntity
    {
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int TopicId { get; set; }
        public string UniqueKey { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Badge { get; set; }
        public string Image { get; set; }
        public string To { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
