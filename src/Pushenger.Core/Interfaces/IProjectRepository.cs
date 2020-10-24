using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;

namespace Pushenger.Core.Interfaces
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Bu isimde bir proje var mı kontrol etmek için kullanılır.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        IDataResult<Project> CheckProject(int companyId, string name);
        /// <summary>
        /// Proje oluşturmak için kullanılır.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IResult Insert(Project project);
    }
}
