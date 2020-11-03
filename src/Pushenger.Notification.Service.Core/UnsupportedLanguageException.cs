using System;

namespace Pushenger.Notification.Service.Core
{
    public class UnsupportedLanguageException : Exception
    {
        public UnsupportedLanguageException():base("Language is not supported")
        {

        }
    }
}
