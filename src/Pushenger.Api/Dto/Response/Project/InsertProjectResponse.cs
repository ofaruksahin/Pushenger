namespace Pushenger.Api.Dto.Response.Project
{
    /// <summary>
    /// Proje oluşturma işlemi response
    /// </summary>
    public class InsertProjectResponse
    {
        public int Id { get; set; }
        public string UniqueKey { get; set; }
        public string SenderKey { get; set; }
        public string TopicUniqueKey { get; set; }
    }
}
