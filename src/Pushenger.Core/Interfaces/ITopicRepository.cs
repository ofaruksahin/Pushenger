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
        /// <summary>
        /// Aktif Topic getirmek için kullanılır
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IDataResult<Topic> Get(int id);
        /// <summary>
        /// Topic güncellemek için kullanılır
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        IResult Update(Topic topic);
        /// <summary>
        /// Topic silmek için kullanılır
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        IResult Delete(Topic topic);
        /// <summary>
        /// UniqueKey göre topic getirme işlemi
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <returns></returns>
        IDataResult<Topic> GetTopicWithUniqueKey(string uniqueKey);
        /// <summary>
        /// Projeye Ait Default Bir Topic Getirir.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IDataResult<Topic> GetDefaultTopic(int projectId);
    }
}
