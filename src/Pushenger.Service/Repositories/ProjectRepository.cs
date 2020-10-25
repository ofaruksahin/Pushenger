using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Collections.Generic;
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
            Project project = connection.ExecuteCommand<Project>("SELECT * FROM project WHERE CompanyId = @companyId AND name = @name AND Status = 1", companyId, name)?.FirstOrDefault();
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

        public IDataResult<Project> GetProject(int id)
        {
            Project project = connection.ExecuteCommand<Project>("SELECT * FROM project WHERE Id = @id AND Status = 1", id)?.FirstOrDefault();
            if (project != null)
                return new SuccessDataResult<Project>(project);
            return new ErrorDataResult<Project>(null,Constant.ProjectMessages.ProjectNotFound);
        }

        public IResult Delete(Project project)
        {
            bool isDeleted = connection.Delete(project);
            if (isDeleted)
                return new SuccessResult();
            return new ErrorResult(Constant.ProjectMessages.ProjectNotDeleted);            
        }

        public IDataResult<List<Project>> GetProjects(int userId)
        {
            List<Project> projects = connection.ExecuteCommand<Project>(@"
            SELECT * FROM project p WHERE p.Id IN
            (SELECT pur.projectId FROM projectuserrel pur WHERE pur.UserId = @userId AND pur.Status = 1)
            AND p.Status = 1", userId).ToList();
            if (projects == null || !projects.Any())
                return new ErrorDataResult<List<Project>>(null, Constant.ProjectMessages.NotFoundAssignedProject);
            return new SuccessDataResult<List<Project>>(projects);
        }

        public IResult Update(Project project)
        {
            bool isUpdated = connection.Update(project);
            if (isUpdated)
                return new SuccessResult();
            return new ErrorResult(Constant.ProjectMessages.ProjectNotUpdated);
        }
    }
}
