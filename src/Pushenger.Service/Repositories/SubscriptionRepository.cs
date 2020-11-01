using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;

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
    }
}
