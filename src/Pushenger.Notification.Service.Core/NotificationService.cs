using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pushenger.Notification.Service.Core
{
    public class NotificationService : INotificationService
    {
        string UniqueKey { get; set; }
        string SenderKey { get; set; }
        string LanguageCode { get; set; }

        HttpClient httpClient { get; set; }

        Uri url = new Uri("http://localhost:51291/api/notification/send");

        public NotificationService(string _UniqueKey, string _SenderKey, SupportedLanguage language)
        {
            UniqueKey = _UniqueKey;
            SenderKey = _SenderKey;

            MemberInfo memberInfo = language.GetType().GetMember(language.ToString())
                                             .FirstOrDefault();

            if (memberInfo != null)
            {
                LanguageCodeAttribute languageAttribute = (LanguageCodeAttribute)
                             memberInfo.GetCustomAttributes(typeof(LanguageCodeAttribute), false)
                                       .FirstOrDefault();
                LanguageCode = languageAttribute.LanguageCode;
            }
            else
            {
                throw new UnsupportedLanguageException();
            }

            httpClient = new HttpClient();
            httpClient.BaseAddress = url;
            httpClient.DefaultRequestHeaders.Add("Accept-Language", LanguageCode);
            httpClient.DefaultRequestHeaders.Add("UniqueKey", UniqueKey);
            httpClient.DefaultRequestHeaders.Add("SenderKey", SenderKey);
        }

        public async Task<SendNotificationResponse> SendNotification(SendNotificationModel sendNotificationModel)
        {
            SendNotificationResponse response = null;
            string serialize = JsonConvert.SerializeObject(sendNotificationModel);
            Encoding encoding = Encoding.UTF8;
            string contentType = "application/json";
            StringContent content = new StringContent(serialize,encoding,contentType);
            var result = await httpClient.PostAsync(url, content);
            var responseString = await result.Content.ReadAsStringAsync();
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    response = JsonConvert.DeserializeObject<SendNotificationResponse>(responseString);
                    if(response.StatusCode != HttpStatusCode.OK)
                        throw new Exception("Hata");
                    break;
                case HttpStatusCode.NotFound:
                    throw new Exception("Hata");
                default:
                    throw new Exception("Hata");
            }           
            return response;
        }
    }
}
