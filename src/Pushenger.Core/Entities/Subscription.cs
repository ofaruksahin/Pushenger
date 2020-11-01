using Dapper.Contrib.Extensions;

namespace Pushenger.Core.Entities
{
    [Table("subscription")]
    public class Subscription : BaseEntity
    {
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int TopicId { get; set; }
        public string ConnectionId { get; set; }
        public string OldConnectionId { get; set; }
        public string Os { get; set; }
        public string OsVersion { get; set; }
        public string App { get; set; }
        public string AppVersion { get; set; }
    }
}
