using FluentValidation;

namespace Pushenger.Api.Dto.Request.ProjectUser
{
    /// <summary>
    /// Projeye Kullanıcı Eklemek İçin Kullanılır.
    /// </summary>
    public class InsertProjectUserRequestDTO
    {
        /// <summary>
        /// Eklenecek Kullanıcı Id
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// Projeye kullanıcı eklemek için kullanılır.
    /// </summary>
    public class InsertProjectUserRequestValidator
        : AbstractValidator<InsertProjectUserRequestDTO>
    {
        public InsertProjectUserRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEqual(0);
        }
    }
}
