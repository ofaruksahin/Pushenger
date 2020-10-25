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
            response.DefaultTopicName = topic.Name;
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
            Core.Entities.User currentUser = GetUser;
            IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProject(id);
            if (!projectExists.Success)
                return NotFound(response, localizer[projectExists.Message]);
            IDataResult<ProjectUserRel> authorizationExists = unitOfWork.ProjectUserRepository.CheckProjectUser(id, currentUser.Id);
            if (!authorizationExists.Success)
                return NotFound(response, projectUserLocalizer[authorizationExists.Message]);
            IResult projectDeleted = unitOfWork.ProjectRepository.Delete(projectExists.Data);
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
            Core.Entities.User currentUser = GetUser;
            IDataResult<Project> projectExists =  unitOfWork.ProjectRepository.GetProject(id);
            if (!projectExists.Success)
                return NotFound(response, localizer[projectExists.Message]);
            IDataResult<ProjectUserRel> authorizationExists = unitOfWork.ProjectUserRepository.CheckProjectUser(id, currentUser.Id);
            if (!authorizationExists.Success)
                return NotFound(response, projectUserLocalizer[authorizationExists.Message]);
            Project project = projectExists.Data;
            project.UniqueKey = Guid.NewGuid().ToString();
            project.SenderKey = Guid.NewGuid().ToString();
            IResult projectUpdated = unitOfWork.ProjectRepository.Update(project);
            if (!projectUpdated.Success)
                return NotFound(response, localizer[projectUpdated.Message]);
            unitOfWork.Commit();
            response.Project = project;
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
            IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProject(id);
            if (!projectExists.Success)
                return NotFound(response, localizer[projectExists.Message]);
            Core.Entities.User currentUser = GetUser;
            IDataResult<ProjectUserRel> authorizationExists = unitOfWork.ProjectUserRepository.CheckProjectUser(id, currentUser.Id);
            if (!authorizationExists.Success)
                return NotFound(response, projectUserLocalizer[authorizationExists.Message]);
            response.Project = projectExists.Data;
            IDataResult<List<Topic>> projectTopics = unitOfWork.TopicRepository.List(id);
            response.Topics = projectTopics.Data;
            IDataResult<List<User>> projectUsers = unitOfWork.ProjectUserRepository.GetUsers(id);
            response.Users = projectUsers.Data;
            return Ok(response);
        }
    }
}
