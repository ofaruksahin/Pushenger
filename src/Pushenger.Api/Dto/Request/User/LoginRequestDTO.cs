using FluentValidation;

namespace Pushenger.Api.Dto.Request.User
{
    /// <summary>
    /// Giriş Yapmak İçin Kullanılır.
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// Email Adresi
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Şifre
        /// </summary>
        public string Password { get; set; }
    }

    public class LoginRequestValidator:
        AbstractValidator<LoginRequestDTO>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
        }
    }
}
