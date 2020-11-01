using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;

namespace Pushenger.Core.Interfaces
{
    public interface ISubscriptionRepository
    {
        /// <summary>
        /// Kullanıcı Subscribe Ekleme İşlemi İçin Kullanılır
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        IResult Insert(Subscription subscription);
    }
}
