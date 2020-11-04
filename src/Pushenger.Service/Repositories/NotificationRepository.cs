using Newtonsoft.Json;
using Pushenger.Core.Entities;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        public IDataResult<Notification> GetNotificationRedis(string uniqueKey)
        {
            var db = cache.GetDatabase((int)enumRedisDatabase.notification);
            bool isExist = db.HashExists("notifications", uniqueKey);
            if (!isExist)
                return new ErrorDataResult<Notification>(null,Constant.NotificationMessage.NotificaitonNotFound);
            var hashValue =  db.HashGet("notifications", uniqueKey);
            try
            {
                Notification notification = JsonConvert.DeserializeObject<Notification>(hashValue.ToString());
                return new SuccessDataResult<Notification>(notification);
            }
            catch (System.Exception)
            {
                return new ErrorDataResult<Notification>(null,Constant.NotificationMessage.NotificaitonNotFound);
            }            
        }

        public IDataResult<List<Notification>> GetNotificationsRedis()
        {
            var db = cache.GetDatabase((int)enumRedisDatabase.notification);
            bool isExist = db.KeyExists("notifications"); 
            if(!isExist)
                return new ErrorDataResult<List<Notification>>(null, Constant.NotificationMessage.NotificaitonNotFound);
            var hashValue = db.HashGetAll("notifications");
            try
            {
                var result = hashValue.Select(x => JsonConvert.DeserializeObject<Notification>(x.Value)).ToList();
                return new SuccessDataResult<List<Notification>>(result);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<Notification>>(null, Constant.NotificationMessage.NotificaitonNotFound);
            }
        }

        #region NotificationDetail
        public IResult InsertNotificationDetailRedis(NotificationDetail notificationDetail)
        {
            try
            {
                var db = cache.GetDatabase((int)enumRedisDatabase.notification);
                db.HashSet(notificationDetail.NotificationId.ToString(), Guid.NewGuid().ToString(), JsonConvert.SerializeObject(notificationDetail));
                return new SuccessResult();
            }
            catch (System.Exception)
            {
                return new ErrorResult();
            }
        }

        public IDataResult<List<NotificationDetail>> GetNotificationDetailsRedis(int notificationId)

        {
            var db = cache.GetDatabase((int)enumRedisDatabase.notification);
            var key = notificationId.ToString();
            var exists = db.KeyExists(key);
            if (!exists)
                return new ErrorDataResult<List<NotificationDetail>>(null);
            var hashValue = db.HashGetAll(key);
            try
            {
                var result = hashValue.Select(x => JsonConvert.DeserializeObject<NotificationDetail>(x.Value)).ToList();
                return new SuccessDataResult<List<NotificationDetail>>(result);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<NotificationDetail>>(null);
            }
        }
        #endregion
    }
}
