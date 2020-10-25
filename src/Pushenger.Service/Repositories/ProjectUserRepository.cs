using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pushenger.Service.Repositories
{
    public class ProjectUserRepository : RepositoryBase, IProjectUserRepository
    {
        public ProjectUserRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }


        public IResult Insert(ProjectUserRel projectUserRel)
        {
            projectUserRel.Id = connection.Insert(projectUserRel);
            if (projectUserRel.Id < 1)
                return new ErrorResult(Constant.ProjectUserMessages.ProjectUserNotCreated);
            return new SuccessResult();            
        }
        public IDataResult<ProjectUserRel> CheckProjectUser(int projectId, int userId)
        {
            ProjectUserRel projectUserRel = connection.ExecuteCommand<ProjectUserRel>("SELECT * FROM projectuserrel WHERE UserId = @userId AND ProjectId = @projectId AND Status = 1;", projectId, userId)?.FirstOrDefault();
            if (projectUserRel != null)
                return new SuccessDataResult<ProjectUserRel>(projectUserRel);
            return new ErrorDataResult<ProjectUserRel>(null,Constant.ProjectUserMessages.UnAuthorized);
        }

        public IDataResult<List<User>> GetUsers(int projectId)
        {
            List<User> users = connection.ExecuteCommand<User>(@"SELECT 
            u.Id,
            u.UserTypeId,
            u.CompanyId,
            u.Name,
            u.Surname,
            u.Email,
            u.CreationDate,
            u.ModifiedDate,
            u.CreatorId,
            u.Status
            FROM user u WHERE u.id IN
            (SELECT pur.UserId FROM projectuserrel pur WHERE pur.ProjectId = @projectId AND pur.Status = 1)
            AND u.Status = 1;", projectId).ToList();
            if (users == null || !users.Any())
                return new ErrorDataResult<List<User>>(null, Constant.ProjectUserMessages.ProjectUsersNotFound);
            return new SuccessDataResult<List<User>>(users);
        }
    }
}
