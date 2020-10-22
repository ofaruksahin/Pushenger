using FluentValidation;
using Pushenger.Core.Enums;

namespace Pushenger.Api.Dto.Request.User
{
    /// <summary>
    /// Kullanıcı Eklemek İçin Kullanılır.
    /// </summary>
    public class InsertUserRequestDTO
    {
        public enumUserType UserTypeId{ get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class InsertUserRequestValidator 
        : AbstractValidator<InsertUserRequestDTO>
    {
        public InsertUserRequestValidator()
        {
            RuleFor(x => x.UserTypeId).NotNull();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
        }
    }
}
