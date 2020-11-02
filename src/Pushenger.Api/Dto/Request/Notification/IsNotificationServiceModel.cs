using FluentValidation;

namespace Pushenger.Api.Dto.Request.Notification
{
    /// <summary>
    /// Proje bilgilerini headersdan almak için kullanılır.
    /// </summary>
    public class IsNotificationServiceModel
    {
        public string UniqueKey { get; set; }
        public string SenderKey { get; set; }
    }

    /// <summary>
    /// Proje bilgilerini validate etmek için kullanılır
    /// </summary>
    public class IsNotificationServiceModelValidator : AbstractValidator<IsNotificationServiceModel>
    {
        public IsNotificationServiceModelValidator()
        {
            RuleFor(x => x.UniqueKey).NotEmpty();
            RuleFor(x => x.UniqueKey).NotEmpty();
        }
    }
}
