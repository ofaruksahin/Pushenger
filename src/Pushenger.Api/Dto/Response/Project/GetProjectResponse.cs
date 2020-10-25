using Pushenger.Core.Entities;
using System.Collections.Generic;

namespace Pushenger.Api.Dto.Response.Project
{
    public class GetProjectResponse
    {
        public Core.Entities.Project Project { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Core.Entities.User> Users { get; set; }
    }
}
