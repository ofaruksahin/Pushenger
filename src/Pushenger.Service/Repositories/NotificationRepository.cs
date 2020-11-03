using Newtonsoft.Json;
using Pushenger.Core.Entities;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;

namespace Pushenger.Service.Repositories
{
    public class NotificationRepository : RepositoryBase, INotificationRepository
    {
        public NotificationRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }

        public IResult Insert(Notification notification)
        {
            notification.Id = connection.Insert(notification);
            if (notification.Id < 1)
                return new ErrorResult(Constant.NotificationMessage.NotificationNotInserted);
            cache.GetDatabase((int)enumRedisDatabase.notification).HashSet("notifications", notification.UniqueKey, JsonConvert.SerializeObject(notification));
            return new SuccessResult();
        }
    }
}
