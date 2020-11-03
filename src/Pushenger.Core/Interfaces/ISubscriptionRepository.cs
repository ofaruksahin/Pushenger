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
        /// <summary>
        /// Bağlantı Anahtarına Göre Kullanıcı Bulma İçin Kullanılır.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        IDataResult<Subscription> GetSubscriptionWithConnectionId(string connectionId);
        /// <summary>
        /// Subscribe bilgilerini günceller
        /// </summary>
        /// <param name="subscription"></param>
        /// <returns></returns>
        IResult Update(Subscription subscription);
        /// <summary>
        /// İlgili connection id değerine göre ilgili kaydı getirir.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        IDataResult<Subscription> GetSubscriptionConnectionIdAndOldConnectionId(string connectionId);
    }
}
