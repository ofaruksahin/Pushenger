using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;

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
    }
}
