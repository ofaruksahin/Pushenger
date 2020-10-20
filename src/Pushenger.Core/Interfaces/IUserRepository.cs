using Pushenger.Core.Entities;
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
        /// <summary>
        /// Email ve şifreye ait kullanıcı var mı kontrol etmek için kullanılır.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IDataResult<User> FindByUser(string email, string password);
        /// <summary>
        /// Giriş Yapmak İçin Kullanılır
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IDataResult<string> LogIn(User user);
    }
}
