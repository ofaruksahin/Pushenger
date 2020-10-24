using Dapper.Contrib.Extensions;

namespace Pushenger.Core.Entities
{
    [Table("topic")]
    public class Topic : BaseEntity
    {
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
