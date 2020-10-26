using FluentValidation;

namespace Pushenger.Api.Dto.Request.Topic
{
    /// <summary>
    /// Topic güncellemek için kullanılır.
    /// </summary>
    public class UpdateTopicRequestDTO
    {
        /// <summary>
        /// Topic adı
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Topic güncelleme model validation
    /// </summary>
    public class UpdateTopicRequestValidator  : AbstractValidator<UpdateTopicRequestDTO>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpdateTopicRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
