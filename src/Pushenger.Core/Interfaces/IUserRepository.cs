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
        /// <summary>
        /// Aktif bir token var mı kontrol eder.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IDataResult<User> CheckToken(string token);
        /// <summary>
        /// İstenilen Id'ye Göre Aktif Kullanıcıyı Getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IDataResult<User> GetUser(int id);
        /// <summary>
        /// Kullanıcı Güncellemek İçin Kullanılır
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IResult UpdateUser(User user);
        /// <summary>
        /// Oturum Kapatmak İçin Kullanılır
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IResult LogOutUser(string token);
        /// <summary>
        /// Kullanıcı eklemek için kullanılır.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IResult Insert(User user);
        /// <summary>
        /// Kullanıcı Silmek İçin Kullanılır
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        IResult Delete(User user);
    }
}
