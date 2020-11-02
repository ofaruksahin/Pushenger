using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.Notification;
using Pushenger.Api.Filters;
using Pushenger.Core.Interfaces;

namespace Pushenger.Api.Controllers
{
    public class NotificationController : BaseController
    {
        public NotificationController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper, 
            IStringLocalizer<BaseResource> _baseLocalizer) 
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
        }

        [HttpPost("send")]
        [IsNotificationService]
        public IActionResult Send([FromBody]SendNotificationRequestDTO dto)
        {
            return new JsonResult(new { });
        }
    }
}
