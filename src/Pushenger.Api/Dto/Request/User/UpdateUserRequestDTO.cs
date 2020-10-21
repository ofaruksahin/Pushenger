using FluentValidation;

namespace Pushenger.Api.Dto.Request.User
{
    public class UpdateUserRequestDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UnHashedPassword { get; set; }
    }

    public class UpdateUserRequestValidator : 
        AbstractValidator<UpdateUserRequestDTO>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);
        }
    }
}
