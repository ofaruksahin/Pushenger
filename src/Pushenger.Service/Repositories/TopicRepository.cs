using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;

namespace Pushenger.Service.Repositories
{
    public class TopicRepository : RepositoryBase, ITopicRepository
    {
        public TopicRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }

        public IResult Insert(Topic topic)
        {
            topic.Id = connection.Insert(topic);
            if (topic.Id < 1)
                return new ErrorResult(Constant.TopicMessages.TopicNotCreated);
            return new SuccessResult();            
        }
    }
}
