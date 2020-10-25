using Dapper.Contrib.Extensions;
using System;

namespace Pushenger.Core.Entities
{
    [Table("project")]
    public class Project : BaseEntity
    {
        public int CompanyId { get; set; }
        public string UniqueKey { get; set; }
        public string SenderKey { get; set; }
        public string Name { get; set; }
    }
}
