using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Resources;
using Pushenger.Core.Interfaces;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Proje kullanıcı işlemlerini yönetmek için kullanılır.
    /// </summary>
    [Route("api/project")]
    public class ProjectUserController : BaseController
    {
        IStringLocalizer<ProjectUserSource> localizer;
        IStringLocalizer<ProjectSource> projectLocalizer;        

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
            IStringLocalizer<ProjectUserSource> _localizer,
            IStringLocalizer<ProjectSource> _projectLocalizer
            ) 
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
            localizer = _localizer;
            projectLocalizer = _projectLocalizer;
        }
    }
}
