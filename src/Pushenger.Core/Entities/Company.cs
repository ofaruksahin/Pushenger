using Dapper.Contrib.Extensions;

namespace Pushenger.Core.Entities
{
    [Table("company")]
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }        
    }
}
