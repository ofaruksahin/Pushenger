namespace Pushenger.Api.Dto.Response.ProjectUser
{
    /// <summary>
    /// Proje kullanıcısı silindi mi sonucusun tutar.
    /// </summary>
    public class DeleteProjectUserResponse
    {
        /// <summary>
        /// Kullanıcı Silindi Mi?
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
