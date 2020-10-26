namespace Pushenger.Api.Dto.Response.Topic
{
    /// <summary>
    /// Topic getirmek için kullanılır.
    /// </summary>
    public class GetTopicResponse
    {
        /// <summary>
        /// Topic
        /// </summary>
        public Core.Entities.Topic topic { get; set; }
    }
}
