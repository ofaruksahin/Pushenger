using FluentValidation;

namespace Pushenger.Api.Dto.Request.Hubs.Subscription
{
    /// <summary>
    /// Bildirim İçin Kullanıcıyı Projeye Subscribe Eder.
    /// </summary>
    public class SubscriptionOnConnected
    {
        public string ProjectKey { get; set; }
        public string TopicKey { get; set; }
        public string Os { get; set; }
        public string OsVersion { get; set; }
        public string App{ get; set; }
        public string AppVersion { get; set; }
        public string OldConnectionId { get; set; }
    }

    /// <summary>
    /// Kullanıcıyı Projeye Subscribe Etme Validator
    /// </summary>
    public class SubscriptionOnConnectedValidator : AbstractValidator<SubscriptionOnConnected>
    {
        public SubscriptionOnConnectedValidator()
        {
            RuleFor(x => x.ProjectKey).NotEmpty();
            RuleFor(x => x.Os).NotEmpty();
            RuleFor(x => x.OsVersion).NotEmpty();
            RuleFor(x => x.App).NotEmpty();
            RuleFor(x => x.AppVersion).NotEmpty();            
        }
    }
}
