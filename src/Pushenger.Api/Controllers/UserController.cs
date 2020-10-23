using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.VisualBasic;
using Pushenger.Api.Dto.Request.User;
using Pushenger.Api.Dto.Response.User;
using Pushenger.Api.Filters;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
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
        [AllowAnonymous]
        public IActionResult LogIn([FromBody] LoginRequestDTO dto)
        {
            LoginResponse response = new LoginResponse();
            IDataResult<User> existUser = unitOfWork.UserRepository.FindByUser(dto.Email, dto.Password);
            if (!existUser.Success)
                return NotFound(response, localizer[existUser.Message]);
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

        /// <summary>
        /// Kullanıcı Adı,Soyadı ve Şifresini Güncellemek İçin Kullanılır.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateUserRequestDTO dto)
        {
            UpdateUserResponse response = new UpdateUserResponse();
            Core.Entities.User user = GetUser;
            user.Name = dto.Name;
            user.Surname = dto.Surname;
            user.UnHashedPassword = dto.UnHashedPassword;
            IResult updateResult = unitOfWork.UserRepository.UpdateUser(user);
            response.IsUpdated = updateResult.Success;
            if (!updateResult.Success)
                return NotFound(response, localizer[updateResult.Message]);
            unitOfWork.Commit();
            return Ok(response);
        }

        /// <summary>
        /// Oturum Kapatma İşlemleri İçin Kullanılır.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            LogOutResponse logOutResponse = new LogOutResponse();            
            IResult result = unitOfWork.UserRepository.LogOutUser(GetToken);
            logOutResponse.IsLogOut = result.Success;
            return Ok(logOutResponse,localizer[result.Message]);            
        }

        /// <summary>
        /// Sistem yöneticisi tarafından o firmaya kullanıcı eklemek için kullanılır.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("insert")]
        [IsOwner]
        public IActionResult Insert([FromBody]InsertUserRequestDTO dto)
        {
            InsertUserResponse response = new InsertUserResponse();
            IResult existUser = unitOfWork.UserRepository.CheckEmail(dto.Email);
            if (existUser.Success)
                return NotFound(response, localizer[existUser.Message]);
            User currentUser = GetUser;
            User user = mapper.Map<User>(dto);
            user.CompanyId = currentUser.CompanyId;
            user.UnHashedPassword = dto.Password;
            user.CreatorId = currentUser.Id;
            IResult insertUser = unitOfWork.UserRepository.Insert(user);
            if (!insertUser.Success)
                return NotFound(response, localizer[insertUser.Message]);
            unitOfWork.Commit();
            response.Id = user.Id;
            return Ok(response);
        }

        /// <summary>
        /// Firma yöneticisi tarafından kullanıcı silmek için kullanılır
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:int}")]
        [IsOwner]
        public IActionResult Delete(int id)
        {
            UserDeleteResponse response = new UserDeleteResponse();
            User currentUser = GetUser;
            IDataResult<User> userResult = unitOfWork.UserRepository.GetUser(id);
            if (!userResult.Success)
                return NotFound(response);
            User user = userResult.Data;
            if (user.CompanyId != currentUser.CompanyId)
                return NotFound(response,localizer[Constant.UserMessages.UserNotFound]);
            if (user.Id == currentUser.Id)
                return NotFound(response,localizer[Constant.UserMessages.UserNotDeletedYourself]);
            IResult userDeleted = unitOfWork.UserRepository.Delete(user);
            if (!userDeleted.Success)
                return NotFound(response, localizer[userDeleted.Message]);
            unitOfWork.Commit();
            response.IsDeleted = userDeleted.Success;
            return Ok(response);
        }
    }
}
