using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.ProjectUser;
using Pushenger.Api.Dto.Response.ProjectUser;
using Pushenger.Api.Filters;
using Pushenger.Api.Resources;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Proje kullanıcı işlemlerini yönetmek için kullanılır.
    /// </summary>
    [Route("api/project")]
    public class ProjectUserController : BaseController
    {
        readonly IStringLocalizer<ProjectUserResource> localizer;
        readonly IStringLocalizer<ProjectResource> projectLocalizer;
        readonly IStringLocalizer<UserResource> userLocalizer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_unitOfWork"></param>
        /// <param name="_mapper"></param>
        /// <param name="_baseLocalizer"></param>
        /// <param name="_localizer"></param>
        /// <param name="_projectLocalizer"></param>
        public ProjectUserController(
            IUnitOfWork _unitOfWork, 
            IMapper _mapper,
            IStringLocalizer<BaseResource> _baseLocalizer,
            IStringLocalizer<ProjectUserResource> _localizer,
            IStringLocalizer<ProjectResource> _projectLocalizer,
            IStringLocalizer<UserResource> _userLocalizer
            ) 
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
            localizer = _localizer;
            projectLocalizer = _projectLocalizer;
            userLocalizer = _userLocalizer;
        }

        /// <summary>
        /// Proje getirme işlemi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [NonAction]
        public object GetProject<T>(T response, int projectId)
        {
            IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProject(projectId);
            if (!projectExists.Success)
                return NotFound(response, localizer[projectExists.Message]);
            Core.Entities.User currentUser = GetUser;
            Project project = projectExists.Data;
            if (project.CompanyId != currentUser.CompanyId)
                return NotFound(response, projectLocalizer[Constant.ProjectMessages.ProjectNotFound]);
            IDataResult<ProjectUserRel> projectUserExists = unitOfWork.ProjectUserRepository.CheckProjectUser(projectId, currentUser.Id);
            if (!projectUserExists.Success)
                return NotFound(response, localizer[projectUserExists.Message]);
            return projectExists.Data;
        }

        /// <summary>
        /// Projeye kullanıcı eklemek için kullanılır.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("{projectId:int}/user/insert")]
        [IsOwner]
        public IActionResult Insert(int projectId,[FromBody]InsertProjectUserRequestDTO dto)
        {
            InsertProjectUserResponse response = new InsertProjectUserResponse();
            object project = GetProject(response, projectId);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;
            Project _project = (Project)project;
            IDataResult<ProjectUserRel> projectUserRelExists = unitOfWork.ProjectUserRepository.CheckProjectUser(projectId, dto.UserId);
            if (projectUserRelExists.Success)
                return NotFound(response, localizer[Constant.ProjectUserMessages.ProjectUserAlreadyExists]);
            Core.Entities.User currentUser = GetUser;
            Core.Entities.User user = unitOfWork.UserRepository.GetUser(dto.UserId).Data;
            if (user == null || (user.CompanyId != currentUser.CompanyId))
                return NotFound(response, userLocalizer[Constant.UserMessages.UserNotFound]);
            ProjectUserRel projectUserRel = new ProjectUserRel()
            {
                CompanyId = currentUser.CompanyId,
                ProjectId = _project.Id,
                UserId = dto.UserId,
                CreatorId = currentUser.Id
            };
            IResult projectUserRelInserted = unitOfWork.ProjectUserRepository.Insert(projectUserRel);
            if (!projectUserRelInserted.Success)
                return NotFound(response, localizer[projectUserRelInserted.Message]);
            unitOfWork.Commit();
            response.Id = projectUserRel.Id;
            return Ok(response);
        }

        /// <summary>
        /// Projeden kullanıcı silmek için kullanılır
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{projectId:int}/user/delete/{userId:int}")]
        [IsOwner]
        public IActionResult Delete(int projectId,int userId)
        {
            DeleteProjectUserResponse response = new DeleteProjectUserResponse();
            object project = GetProject(response, projectId);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;
            Project _project = (Project)project;
            IDataResult<ProjectUserRel> projectUserRel = unitOfWork.ProjectUserRepository.CheckProjectUser(projectId, userId);
            if (!projectUserRel.Success)
                return NotFound(response, localizer[Constant.ProjectUserMessages.ProjectUsersNotFound]);
            Core.Entities.User user = unitOfWork.UserRepository.GetUser(projectUserRel.Data.UserId).Data;
            Core.Entities.User currentUser = GetUser;
            if (user == null)
                return NotFound(response, userLocalizer[Constant.UserMessages.UserNotFound]);
            if (user.Id == currentUser.Id)
                return NotFound(response, userLocalizer[Constant.UserMessages.UserNotDeletedYourself]);
            IResult projectUserDeleted = unitOfWork.ProjectUserRepository.Delete(projectUserRel.Data);
            if (!projectUserDeleted.Success)
                return NotFound(response, localizer[projectUserDeleted.Message]);
            unitOfWork.Commit();
            response.IsDeleted = projectUserDeleted.Success;
            return Ok(response);
        }
    }
}
