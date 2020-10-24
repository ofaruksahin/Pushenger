using FluentValidation;

namespace Pushenger.Api.Dto.Request.Project
{
    /// <summary>
    /// Proje Eklemek İçin Kullanılır.
    /// </summary>
    public class InsertProjectRequestDTO
    {
        /// <summary>
        /// Proje Adı
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Proje eklemek model validator
    /// </summary>
    public class InsertProjectRequestValidator : AbstractValidator<InsertProjectRequestDTO>
    {
        public InsertProjectRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
