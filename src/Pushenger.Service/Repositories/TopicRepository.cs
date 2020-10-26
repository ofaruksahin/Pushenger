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

        public IDataResult<Topic> Get(int id)
        {
            Topic topic = connection.ExecuteCommand<Topic>("SELECT * FROM topic WHERE Id = @id AND Status = 1",id)?.FirstOrDefault();
            if (topic == null)
                return new ErrorDataResult<Topic>(null,Constant.TopicMessages.TopicNotFound);
            return new SuccessDataResult<Topic>(topic);
        }

        public IResult Update(Topic topic)
        {
            bool isUpdated = connection.Update(topic);
            if (isUpdated)
                return new SuccessResult();
            return new ErrorResult(Constant.TopicMessages.TopicNotUpdated);
        }

        public IResult Delete(Topic topic)
        {
            bool isDeleted = connection.Delete(topic);
            if (isDeleted)
                return new SuccessResult();
            return new ErrorResult(Constant.TopicMessages.TopicNotDeleted);
        }
    }
}
