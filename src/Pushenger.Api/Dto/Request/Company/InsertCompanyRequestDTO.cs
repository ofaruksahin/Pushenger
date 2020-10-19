using FluentValidation;

namespace Pushenger.Api.Dto.Request.Company
{
    /// <summary>
    /// Firma oluşturulmak için kullanılır.
    /// </summary>
    public class InsertCompanyRequestDTO
    {
        /// <summary>
        /// Firma yetkili adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Firma yetklisi soyadı
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Firma adı
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// Firma e-posta adresi
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Firma yetkili kullanıcı şifresi
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Firma oluşturma model validator
    /// </summary>
    public class InsertCompanyRequestValidator : 
        AbstractValidator<InsertCompanyRequestDTO>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsertCompanyRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(20);
        }
    }
}
