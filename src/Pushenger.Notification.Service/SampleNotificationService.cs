using Pushenger.Notification.Service.Core;
using System;
using System.Net;
using System.Threading;

namespace Pushenger.Notification.Service
{
    public class SampleNotificationService : NotificationService
    {
        Timer timer;
        object lockObject = new object();

        public SampleNotificationService(
            string _UniqueKey, 
            string _SenderKey, 
            SupportedLanguage language) 
            : 
            base(_UniqueKey, _SenderKey,
                language)
        {
            timer = new Timer(TimerDoWork, lockObject, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        private void TimerDoWork(object state)
        {
            if (!Monitor.TryEnter(lockObject))
                return;
            var response =  SendNotification(new SendNotificationModel
            {
                Title = "Pushenger Service Title",
                Body = "Pushenger Service Body",
                TopicKey = "eeb454a1-310a-44d2-bfbe-3753d3a58b61"
            }).Result;
            if(response != null && response.StatusCode ==HttpStatusCode.OK)
                Console.WriteLine("Bildirim Gönderildi");
            else
                Console.WriteLine("Bildirim Gönderilemedi");
            Monitor.Exit(lockObject);
        }
    }
}
