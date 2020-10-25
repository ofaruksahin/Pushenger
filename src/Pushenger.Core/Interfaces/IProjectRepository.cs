using Pushenger.Core.Entities;
using Pushenger.Core.Utilities.Result;
using System.Collections.Generic;

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
        /// <summary>
        /// Aktif projeyi getirmek için kullanılır.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IDataResult<Project> GetProject(int id);
        /// <summary>
        /// Proje silmek için kullanılır
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IResult Delete(Project project);
        /// <summary>
        /// Kullanıcının Görebileceği projeleri listeler.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IDataResult<List<Project>> GetProjects(int userId);
        /// <summary>
        /// Proje bilgilerini güncellemek için kullanılır.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        IResult Update(Project project);
    }
}
