using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;
using Pushenger.Api.Dto.Request.User;
using Pushenger.Api.Dto.Response.User;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities.Result;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Kullanıcı İşlemlerini Yönetmek İçin Kullanılır.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        readonly IStringLocalizer<UserResource> localizer;

        public UserController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IStringLocalizer<BaseResource> _baseLocalizer,
            IStringLocalizer<UserResource> _localizer
            )
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
            localizer = _localizer;
        }

        /// <summary>
        /// Giriş Yapmak İçin Kullanılır.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult LogIn([FromBody] LoginRequestDTO dto)
        {
            LoginResponse response = new LoginResponse();
            IDataResult<User> existUser = unitOfWork.UserRepository.FindByUser(dto.Email, dto.Password);
            if (!existUser.Success)
                return NotFound(response,localizer[existUser.Message]);
            IDataResult<Company> company = unitOfWork.CompanyRepository.FindById(existUser.Data.CompanyId);
            if (!company.Success)
                return NotFound(response, localizer[company.Message]);
            IDataResult<string> token = unitOfWork.UserRepository.LogIn(existUser.Data);
            if (!token.Success)
                return NotFound(response, localizer[token.Message]);
            response.User = existUser.Data;
            response.Company = company.Data;
            response.Token = token.Data;
            response.ExpireDate = DateAndTime.Now.AddDays(1);
            return Ok(response);
        }
    }
}
