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
                UniqueKey = Guid.NewGuid(),
                SenderKey = Guid.NewGuid(),
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
    }
}
