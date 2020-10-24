using FluentValidation;
using Pushenger.Core.Enums;

namespace Pushenger.Api.Dto.Request.User
{
    /// <summary>
    /// Kullanıcı Tipi Güncellemek İçin Kullanılır.
    /// </summary>
    public class UpdateUserTypeRequestDTO
    {
        /// <summary>
        /// Kullanıcı Tipi
        /// </summary>
        public enumUserType UserType { get; set; }
    }

    public class UpdateUserTypeRequestValidator : AbstractValidator<UpdateUserTypeRequestDTO>
    {
        public UpdateUserTypeRequestValidator()
        {
            RuleFor(x => x.UserType).NotNull();
        }
    }
}
