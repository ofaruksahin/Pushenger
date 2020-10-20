using System;

namespace Pushenger.Api.Dto.Response.User
{
    public class LoginResponse
    {
        public Core.Entities.User User { get; set; }
        public Core.Entities.Company Company { get; set; }
        public string Token { get; set; }
        public DateTime? ExpireDate { get; set; }

    }
}
