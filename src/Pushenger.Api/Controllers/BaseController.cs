using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Models;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using System;
using System.Net;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Paylaşım Katmanı
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController
        : ControllerBase
    {
        /// <summary>
        /// Unit of Work classı
        /// </summary>
        public readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// AutoMapper classı
        /// </summary>
        public readonly IMapper mapper;

        /// <summary>
        /// Base mesajlar için localizaiton servisi
        /// </summary>
        readonly IStringLocalizer<BaseResource> baseLocalizer;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_unitOfWork"></param>
        /// <param name="_mapper"></param>
        /// <param name="_baseLocalizer"></param>
        public BaseController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IStringLocalizer<BaseResource> _baseLocalizer
            )
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            baseLocalizer = _baseLocalizer;
        }


        /// <summary>
        /// İşlem başarılıysa clientlara bu şekilde dönüş sağlanır.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult Ok<T>(T data, string message = "")
        {
            BaseResult<T> baseResult = new BaseResult<T>()
            {
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.OK
            };

            if (String.IsNullOrEmpty(message))
                baseResult.Message = baseLocalizer[Constant.Successful];
            return new OkObjectResult(baseResult);
        }

        /// <summary>
        /// İşlem başarısızsa clientlara bu şekilde dönüş sağlanır.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [NonAction]
        public IActionResult NotFound<T>(T data, string message = "")
        {
            BaseResult<T> baseResult = new BaseResult<T>()
            {
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.NotFound
            };

            if (String.IsNullOrEmpty(message))
                baseResult.Message = baseLocalizer[Constant.Error];
            return new NotFoundObjectResult(baseResult);
        }
    }
}
