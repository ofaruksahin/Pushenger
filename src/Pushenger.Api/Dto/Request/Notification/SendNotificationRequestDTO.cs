using FluentValidation;
using System;

namespace Pushenger.Api.Dto.Request.Notification
{
    /// <summary>
    /// Bildirim gönderme işlemi
    /// </summary>
    public class SendNotificationRequestDTO
    {
        /// <summary>
        /// Bildirim başlığı
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Bildirim içeriği
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// Küçük resim
        /// </summary>
        public string Badge { get; set; }
        /// <summary>
        /// resim
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Belirli bir gruba bildirim göndermek için kullanılır.
        /// </summary>
        public string TopicKey { get; set; }
        /// <summary>
        /// Belirli bir kişiye göndermek için kullanılır.
        /// </summary>
        public string To { get; set; }
    }

    /// <summary>
    /// Bildirim gönderme model validation
    /// </summary>
    public class SendNotificationRequestValidator
        : AbstractValidator<SendNotificationRequestDTO>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public SendNotificationRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Body).NotEmpty().MaximumLength(255);
            RuleFor(x => x.TopicKey).Custom((val, context) =>
            {
                if (!String.IsNullOrEmpty(val))
                {

                    if (!String.IsNullOrEmpty(((SendNotificationRequestDTO)context.InstanceToValidate).To))
                    {
                        context.AddFailure("Bir gruba ve kişiye aynı anda bildirim gönderemezsiniz");
                    }
                }
            }).MaximumLength(255);

            RuleFor(x => x.To).Custom((val, context) =>
            {
                if (!String.IsNullOrEmpty(val))
                {
                    if (!String.IsNullOrEmpty(((SendNotificationRequestDTO)context.InstanceToValidate).TopicKey))
                    {
                        context.AddFailure("Bir gruba ve kişiye aynı anda bildirim gönderemezsiniz");
                    }
                }
            }).MaximumLength(255);
        }
    }
}
