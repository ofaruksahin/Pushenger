using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;
using System.Linq;

namespace Pushenger.Service.Repositories
{
    public class SubscriptionRepository : RepositoryBase, ISubscriptionRepository
    {
        public SubscriptionRepository(IDbTransaction transaction) : base(transaction)
        {
        }        

        public IResult Insert(Subscription subscription)
        {
            subscription.Id = connection.Insert(subscription);
            if (subscription.Id < 1)
                return new ErrorResult(Constant.SubscriptionMessages.SubscriptionNotInserted);
            return new SuccessResult();            
        }

        public IDataResult<Subscription> GetSubscriptionWithConnectionId(string connectionId)
        {
            Subscription subscription = connection.ExecuteCommand<Subscription>("SELECT * FROM subscription WHERE ConnectionId = @connectionId AND Status = 1", connectionId)?.FirstOrDefault();
            if (subscription == null)
                return new ErrorDataResult<Subscription>(null,Constant.SubscriptionMessages.SubscriptionNotFound);
            return new SuccessDataResult<Subscription>(subscription);
        }

        public IResult Update(Subscription subscription)
        {
            bool isUpdated = connection.Update(subscription);
            if (isUpdated)
                return new SuccessResult();
            return new ErrorResult(Constant.SubscriptionMessages.SubscriptionNotUpdated);
        }
    }
}
