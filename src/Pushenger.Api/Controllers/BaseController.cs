using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pushenger.Api.Models;
using Pushenger.Core.Interfaces;
using System.Net;

namespace Pushenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController 
        : ControllerBase
    {
        public readonly IUnitOfWork unitOfWork;

        public readonly IMapper mapper;

        public BaseController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper
            )
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        [NonAction]
        public IActionResult Ok<T>(T data)
        {
            BaseResult<T> baseResult = new BaseResult<T>()
            {
                Data = data,
                Message = "",
                StatusCode = HttpStatusCode.OK
            };

            return new OkObjectResult(baseResult);
        }

        [NonAction]
        public IActionResult NotFound<T>(T data, string message)
        {
            BaseResult<T> baseResult = new BaseResult<T>()
            {
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.NotFound
            };
            return new NotFoundObjectResult(baseResult);
        }
    }
}
