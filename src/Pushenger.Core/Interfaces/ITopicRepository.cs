using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;
using System.Collections.Generic;

namespace Pushenger.Core.Interfaces
{
    public interface ITopicRepository
    {
        /// <summary>
        /// Topic eklemek için kullanılır.
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        IResult Insert(Topic topic);
        /// <summary>
        /// Topic listelemek için kullanılır
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IDataResult<List<Topic>> List(int projectId);
    }
}
