using System;

namespace Pushenger.Notification.Service.Core
{
    public class LanguageCodeAttribute :Attribute
    {
        public string LanguageCode { get; set; }

        public LanguageCodeAttribute(string _LanguageCode)
        {
            LanguageCode = _LanguageCode;
        }
    }
}
