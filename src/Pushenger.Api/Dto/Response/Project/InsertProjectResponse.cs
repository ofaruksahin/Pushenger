using System;

namespace Pushenger.Api.Dto.Response.Project
{
    /// <summary>
    /// Proje oluşturma işlemi response
    /// </summary>
    public class InsertProjectResponse
    {
        public int Id { get; set; }
        public Guid UniqueKey { get; set; }
        public Guid SenderKey { get; set; }
        public string DefaultTopicName { get; set; }
    }
}
