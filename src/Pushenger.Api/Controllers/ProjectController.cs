using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.Project;
using Pushenger.Api.Dto.Response.Project;
using Pushenger.Api.Filters;
using Pushenger.Api.Resources;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System;
using System.Collections.Generic;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Proje İşlemlerini Yönetmek İçin Kullanılır.
    /// </summary>
    public class ProjectController : BaseController
    {
        IStringLocalizer<ProjectSource> localizer;
        IStringLocalizer<TopicSource> topicLocalizer;
        IStringLocalizer<ProjectUserSource> projectUserLocalizer;
        public ProjectController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IStringLocalizer<BaseResource> _baseLocalizer,
            IStringLocalizer<ProjectSource> _localizer,
            IStringLocalizer<TopicSource> _topicLocalizer,
            IStringLocalizer<ProjectUserSource> _projectUserLocalizer
            )
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
            localizer = _localizer;
            topicLocalizer = _topicLocalizer;
            projectUserLocalizer = _projectUserLocalizer;
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
                return NotFound(response, localizer[Constant.ProjectMessages.ProjectNotFound]);
            IDataResult<ProjectUserRel> projectUserExists = unitOfWork.ProjectUserRepository.CheckProjectUser(projectId, currentUser.Id);
            if (!projectUserExists.Success)
                return NotFound(response, projectUserLocalizer[projectUserExists.Message]);
            return projectExists.Data;
        }

        /// <summary>
        /// Proje Oluşturmak İçin Kullanılır.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Insert")]
        [IsOwner]
        public IActionResult Insert([FromBody] InsertProjectRequestDTO dto)
        {
            InsertProjectResponse response = new InsertProjectResponse();
            Core.Entities.User currentUser = GetUser;
            IDataResult<Project> existsProject = unitOfWork.ProjectRepository.CheckProject(currentUser.CompanyId, dto.Name);
            if (!existsProject.Success)
                return NotFound(response, localizer[existsProject.Message]);
            Project project = new Project()
            {
                CompanyId = currentUser.CompanyId,
                UniqueKey = Guid.NewGuid().ToString(),
                SenderKey = Guid.NewGuid().ToString(),
                Name = dto.Name,
                CreatorId = currentUser.Id,
            };
            IResult projectCreated = unitOfWork.ProjectRepository.Insert(project);
            if (!projectCreated.Success)
                return NotFound(response, localizer[projectCreated.Message]);
            Topic topic = new Topic()
            {
                CompanyId = currentUser.CompanyId,
                ProjectId = project.Id,
                UniqueKey = Guid.NewGuid().ToString(),
                Name = Constant.DefaultTopicName,
                IsDefault = true,
                CreatorId = currentUser.Id
            };
            IResult topicCreated = unitOfWork.TopicRepository.Insert(topic);
            if (!topicCreated.Success)
                return NotFound(response, topicLocalizer[topicCreated.Message]);
            ProjectUserRel projectUser = new ProjectUserRel()
            {
                CompanyId = currentUser.CompanyId,
                ProjectId = project.Id,
                UserId = currentUser.Id,
                CreatorId = currentUser.Id
            };
            IResult userAdded = unitOfWork.ProjectUserRepository.Insert(projectUser);
            if (!userAdded.Success)
                return NotFound(response, projectUserLocalizer[userAdded.Message]);
            response.Id = project.Id;
            response.UniqueKey = project.UniqueKey;
            response.SenderKey = project.SenderKey;
            response.TopicUniqueKey = topic.UniqueKey;
            unitOfWork.Commit();
            return Ok(response);
        }

        /// <summary>
        /// Proje silmek için kullanılır.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:int}")]
        [IsOwner]
        public IActionResult Delete(int id)
        {
            DeleteProjectResponse response = new DeleteProjectResponse();
            object project = GetProject(response, id);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;
            IResult projectDeleted = unitOfWork.ProjectRepository.Delete((Project)project);
            if (!projectDeleted.Success)
                return NotFound(response, localizer[projectDeleted.Message]);
            unitOfWork.Commit();
            response.IsDeleted = projectDeleted.Success;
            return Ok(response);
        }

        /// <summary>
        /// Yetkili olunan projeleri görüntülemek için kullanılır.
        /// </summary>
        [HttpGet("list")]
        public IActionResult List()
        {
            ListProjectResponse response = new ListProjectResponse();
            Core.Entities.User currentUser = GetUser;
            IDataResult<List<Project>> userProjects = unitOfWork.ProjectRepository.GetProjects(currentUser.Id);
            if (!userProjects.Success)
                return NotFound(response, localizer[userProjects.Message]);
            response.Projects = userProjects.Data;
            return Ok(response);
        }

        /// <summary>
        /// Projeye ait uniquekey ve senderkey parametrelerini güncellemek için oluşturulur.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("regeneratekey/{id:int}")]
        [IsOwner]
        public IActionResult ReGenerateKey(int id)
        {
            ReGenerateKeyResponse response = new ReGenerateKeyResponse();
            object project = GetProject(response, id);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;            
            Project _project = (Project)project;
            _project.UniqueKey = Guid.NewGuid().ToString();
            _project.SenderKey = Guid.NewGuid().ToString();
            IResult projectUpdated = unitOfWork.ProjectRepository.Update(_project);
            if (!projectUpdated.Success)
                return NotFound(response, localizer[projectUpdated.Message]);
            unitOfWork.Commit();
            response.Project = _project;
            return Ok(response);
        }

        /// <summary>
        /// Projeye Ait Bilgileri Getirmek İçin Kullanılır.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id:int}")]
        public IActionResult Get(int id)
        {
            GetProjectResponse response = new GetProjectResponse();
            object project = GetProject(response, id);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;          
            response.Project = (Project)project;
            IDataResult<List<Topic>> projectTopics = unitOfWork.TopicRepository.List(id);
            response.Topics = projectTopics.Data;
            IDataResult<List<User>> projectUsers = unitOfWork.ProjectUserRepository.GetUsers(id);
            response.Users = projectUsers.Data;
            return Ok(response);
        }
    }
}
