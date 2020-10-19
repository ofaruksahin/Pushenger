using Dapper.Contrib.Extensions;
using Pushenger.Core.Enums;

namespace Pushenger.Core.Entities
{
    [Table("user")]
    public class User : BaseEntity
    {
        public enumUserType UserTypeId { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  
        
        [Write(false)]
        public string UnHashedPassword { get; set; }
    }
}
