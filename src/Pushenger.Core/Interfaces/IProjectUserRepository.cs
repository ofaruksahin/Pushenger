using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;

namespace Pushenger.Core.Interfaces
{
    public interface IProjectUserRepository
    {
        /// <summary>
        /// Projeye Kullanıcı Eklemek İçin Kullanılır.
        /// </summary>
        /// <param name="projectUserRel"></param>
        /// <returns></returns>
        IResult Insert(ProjectUserRel projectUserRel);
    }
}
