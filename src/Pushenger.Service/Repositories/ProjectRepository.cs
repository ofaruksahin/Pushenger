using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;
using System.Linq;

namespace Pushenger.Service.Repositories
{
    public class ProjectRepository : RepositoryBase, IProjectRepository
    {
        public ProjectRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }

        public IDataResult<Project> CheckProject(int companyId, string name)
        {
            Project project = connection.ExecuteCommand<Project>("SELECT * FROM project WHERE CompanyId = @companyId AND name = @name AND Status = 1", companyId, name).FirstOrDefault();
            if (project == null)
                return new SuccessDataResult<Project>(project);
            return new ErrorDataResult<Project>(null, Constant.ProjectMessages.ProjectAlreadyExists);
        }

        public IResult Insert(Project project)
        {
            project.Id = connection.Insert(project);
            if (project.Id < 1)
                return new ErrorResult(Constant.ProjectMessages.ProjectNotCreated);
            return new SuccessResult();
        }
    }
}
