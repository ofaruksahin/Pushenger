using Pushenger.Core.Utilities.Result;

namespace Pushenger.Core.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Bu bilgilere ait aktif kullanıcı var mı kontrol etmek için kullanılır.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        IResult CheckEmail(string email);
    }
}
