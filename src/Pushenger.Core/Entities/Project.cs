using Dapper.Contrib.Extensions;
using System;

namespace Pushenger.Core.Entities
{
    [Table("project")]
    public class Project : BaseEntity
    {
        public int CompanyId { get; set; }
        public Guid UniqueKey { get; set; }
        public Guid SenderKey { get; set; }
        public string Name { get; set; }
    }
}
