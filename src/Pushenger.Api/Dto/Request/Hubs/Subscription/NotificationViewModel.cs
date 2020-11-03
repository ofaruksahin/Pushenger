namespace Pushenger.Api.Dto.Request.Hubs.Subscription
{
    public class NotificationViewModel
    {
        public string UniqueKey { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Badge { get; set; }
        public string Image { get; set; }
    }
}
