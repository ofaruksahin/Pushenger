using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;

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
    }
}
