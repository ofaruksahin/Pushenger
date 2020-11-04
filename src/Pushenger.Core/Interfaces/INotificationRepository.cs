using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;
using System.Collections.Generic;

namespace Pushenger.Core.Interfaces
{
    public interface INotificationRepository
    {
        /// <summary>
        /// Bildirim eklemek için kullanılır.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        IResult Insert(Notification notification);
        /// <summary>
        /// Redisten bildirimi getirmek için kullanılır
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <returns></returns>
        IDataResult<Notification> GetNotificationRedis(string uniqueKey);
        /// <summary>
        /// Redisteki bildirimleri almak için kullanılır
        /// </summary>
        /// <returns></returns>
        IDataResult<List<Notification>> GetNotificationsRedis();

        #region NotificationDetails        
        /// <summary>
        /// Redis üzerine notification detail eklemek için kullanılır
        /// </summary>
        /// <param name="notificationDetail"></param>
        /// <returns></returns>
        IResult InsertNotificationDetailRedis(NotificationDetail notificationDetail);
        IDataResult<List<NotificationDetail>> GetNotificationDetailsRedis(int notificationId);
        #endregion
    }
}
