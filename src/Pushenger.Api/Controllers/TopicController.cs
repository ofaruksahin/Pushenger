using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.Topic;
using Pushenger.Api.Dto.Response.Topic;
using Pushenger.Api.Filters;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Proje topic işlemleri için kullanılır.
    /// </summary>
    [Route("api/project")]
    public class TopicController : BaseController
    {
        IStringLocalizer<TopicSource> localizer;
        IStringLocalizer<ProjectSource> projectLocalizer;
        IStringLocalizer<ProjectUserRel> projectUserLocalizer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_unitOfWork"></param>
        /// <param name="_mapper"></param>
        /// <param name="_baseLocalizer"></param>
        /// <param name="_localizer"></param>
        /// <param name="_projectLocalizer"></param>
        public TopicController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IStringLocalizer<BaseResource> _baseLocalizer,
            IStringLocalizer<TopicSource> _localizer,
            IStringLocalizer<ProjectSource> _projectLocalizer,
            IStringLocalizer<ProjectUserRel> _projectUserLocalizer
            )
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
            localizer = _localizer;
            projectLocalizer = _projectLocalizer;
            projectUserLocalizer = _projectUserLocalizer;
        }

        /// <summary>
        /// Proje Getirme İşlemi
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [NonAction]
        public object GetProject<T>(T response,int projectId)
        {
            IDataResult<Project> projectExists = unitOfWork.ProjectRepository.GetProject(projectId);
            if (!projectExists.Success)
                return NotFound(response, projectLocalizer[projectExists.Message]);
            Core.Entities.User currentUser = GetUser;
            Project project = projectExists.Data;
            if (project.CompanyId != currentUser.CompanyId)
                return NotFound(response, projectLocalizer[Constant.ProjectMessages.ProjectNotFound]);
            IDataResult<ProjectUserRel> projectUserExists = unitOfWork.ProjectUserRepository.CheckProjectUser(projectId, currentUser.Id);
            if (!projectUserExists.Success)
                return NotFound(response, projectUserLocalizer[projectUserExists.Message]);
            return projectExists.Data;
        }

        /// <summary>
        /// Topic bilgilerini getirmek için kullanılır.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpGet("{projectId:int}/topic/get/{topicId:int}")]
        public IActionResult Get(int projectId,int topicId)
        {
            GetTopicResponse response = new GetTopicResponse();
            object project = GetProject(response, projectId);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;
            IDataResult<Topic> topicExists = unitOfWork.TopicRepository.Get(topicId);
            if (!topicExists.Success)
                return NotFound(response, localizer[topicExists.Message]);
            response.topic = topicExists.Data;
            return Ok(response);
        }
        
        /// <summary>
        /// Topic oluşturmak için kullanılır.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("{projectId:int}/topic/insert")]
        [IsOwner]
        public IActionResult Insert(int projectId,[FromBody]InsertTopicRequestDTO dto)
        {
            InsertTopicResponse response = new InsertTopicResponse();
            object project = GetProject(response, projectId);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;
            Core.Entities.User currentUser = GetUser;
            var _project = (Project)project;
            Topic topic = new Topic()
            {
                CompanyId = _project.CompanyId,
                ProjectId = _project.Id,
                UniqueKey = Guid.NewGuid().ToString(),
                Name = dto.Name,
                IsDefault = false,
                CreatorId = currentUser.Id,
            };
            IResult topicCreated = unitOfWork.TopicRepository.Insert(topic);
            if (!topicCreated.Success)
                return NotFound(response, localizer[topicCreated.Message]);
            unitOfWork.Commit();
            response.Id = topic.Id;
            return Ok(response);
        }

        /// <summary>
        /// Topic güncellemek için kullanılır
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="topicId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{projectId:int}/topic/update/{topicId:int}")]
        [IsOwner]
        public IActionResult Update(int projectId,int topicId,[FromBody]UpdateTopicRequestDTO dto)
        {
            UpdateTopicResponse response = new UpdateTopicResponse();
            object project = GetProject(response, projectId);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;
            IDataResult<Topic> topicExists = unitOfWork.TopicRepository.Get(topicId);
            if (!topicExists.Success)
                return NotFound(response, localizer[topicExists.Message]);
            Topic topic = topicExists.Data;            
            topic.Name = dto.Name;
            IResult topicUpdated = unitOfWork.TopicRepository.Update(topic);
            if (!topicUpdated.Success)
                return NotFound(response, localizer[topicUpdated.Message]);
            unitOfWork.Commit();
            response.IsUpdated = topicUpdated.Success;
            return Ok(response);
        }

        /// <summary>
        /// Topic silmek için kullanılır
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpDelete("{projectId:int}/topic/delete/{topicId:int}")]
        [IsOwner]
        public IActionResult Delete(int projectId,int topicId)
        {
            DeleteTopicResponse response = new DeleteTopicResponse();
            object project = GetProject(response, projectId);
            if (project.GetType().BaseType == typeof(ObjectResult))
                return (IActionResult)project;
            IDataResult<Topic> topicExists = unitOfWork.TopicRepository.Get(topicId);
            if (!topicExists.Success)
                return NotFound(response, localizer[topicExists.Message]);
            Topic topic = topicExists.Data;
            if (topic.IsDefault)
                return NotFound(response,localizer[Constant.TopicMessages.DefaultGroupNotDeleted]);
            IResult topicDeleted = unitOfWork.TopicRepository.Delete(topic);
            if (!topicDeleted.Success)
                return NotFound(response, localizer[topicDeleted.Message]);
            unitOfWork.Commit();
            response.IsDeleted = topicDeleted.Success;
            return Ok(response);
        }
    }
}
