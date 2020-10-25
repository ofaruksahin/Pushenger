﻿using Pushenger.Core.Entities;
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
        /// <summary>
        /// Kullanıcı proje ile yetkili mi kontrol etmek için kullanılır.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IDataResult<ProjectUserRel> CheckProjectUser(int projectId, int userId);
    }
}
