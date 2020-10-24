using Dapper.Contrib.Extensions;

namespace Pushenger.Core.Entities
{
    [Table("projectuserrel")]
    public class ProjectUserRel : BaseEntity
    {
        public int CompanyId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }        
    }
}
