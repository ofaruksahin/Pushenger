namespace Pushenger.Api.Dto.Response.User
{
    /// <summary>
    /// Oturum Kapatma İşlemleri İçin Kullanılır.
    /// </summary>
    public class LogOutResponse
    {
        /// <summary>
        /// Oturum kapatılma durumunu tutar.
        /// </summary>
        public bool IsLogOut { get; set; }
    }
}
