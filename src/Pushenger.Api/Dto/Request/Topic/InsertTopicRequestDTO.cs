using FluentValidation;

namespace Pushenger.Api.Dto.Request.Topic
{
    /// <summary>
    /// Topic oluşturmak için kullanılır.
    /// </summary>
    public class InsertTopicRequestDTO
    {
        /// <summary>
        /// Topic adı
        /// </summary>
        public string Name { get; set; }        
    }

    /// <summary>
    /// Topic oluşturma model validator
    /// </summary>
    public class InsertTopicRequestValidator 
        : AbstractValidator<InsertTopicRequestDTO>
    {
        /// <summary>
        /// 
        /// </summary>
        public InsertTopicRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
