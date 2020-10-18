using System.Net;

namespace Pushenger.Api.Models
{
    public class BaseResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public BaseResult()
        {
            Data = default;
            Message = default;
            StatusCode = HttpStatusCode.OK;
        }
    }
}
