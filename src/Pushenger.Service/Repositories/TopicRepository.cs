using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        public IDataResult<List<Topic>> List(int projectId)
        {
            List<Topic> topics = connection.ExecuteCommand<Topic>("SELECT * FROM topic WHERE ProjectId = @projectId AND Status = 1;", projectId).ToList();
            if (topics == null || !topics.Any())
                return new ErrorDataResult<List<Topic>>(null, Constant.TopicMessages.ProjectTopicsNotFound);
            return new SuccessDataResult<List<Topic>>(topics);
        }
    }
}
