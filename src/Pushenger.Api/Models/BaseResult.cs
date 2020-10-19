using System;
using System.Net;

namespace Pushenger.Api.Models
{
    /// <summary>
    /// Clientlara cevap olarak dönmesi gereken ortak model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResult<T>
    {
        /// <summary>
        /// Clientlara iletilecek data
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Clientlara iletilecek mesaj
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Clientlara iletilecek hata kodu
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseResult()
        {
            Data = (T)Activator.CreateInstance(typeof(T));
            Message = default;
            StatusCode = HttpStatusCode.OK;
        }
    }
}
