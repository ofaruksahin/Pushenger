using FluentValidation;

namespace Pushenger.Api.Dto.Request.Company
{
    /// <summary>
    /// Firma bilgilerini güncellemek için kullanılır.
    /// </summary>
    public class UpdateCompanyRequestDTO
    {
        /// <summary>
        /// Firma yetkili adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Firma yetkili soyadı
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// Firma adı
        /// </summary>
        public string CompanyName { get; set; }
    }

    /// <summary>
    // Firma güncelleme model validator
    /// </summary>
    public class UpdateCompanyRequestValidator: AbstractValidator<UpdateCompanyRequestDTO>
    {
        public UpdateCompanyRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(255);
        }
    }
}
