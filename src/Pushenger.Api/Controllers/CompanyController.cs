using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Pushenger.Api.Dto.Request.Company;
using Pushenger.Api.Dto.Response.Company;
using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities.Result;

namespace Pushenger.Api.Controllers
{
    /// <summary>
    /// Firma Yönetim İşlemlerini Yapar.
    /// </summary>
    public class CompanyController
        : BaseController
    {
        readonly IStringLocalizer<CompanyResource> localizer;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_unitOfWork"></param>
        /// <param name="_mapper"></param>
        /// <param name="_baseLocalizer"></param>
        /// <param name="_localizer"></param>
        public CompanyController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IStringLocalizer<BaseResource> _baseLocalizer,
            IStringLocalizer<CompanyResource> _localizer
            )
            : base(_unitOfWork, _mapper, _baseLocalizer)
        {
            localizer = _localizer;
        }

        /// <summary>
        /// Yeni bir firma ve o firmaya ait yönetici kullanıcısı oluşturmak için kullanılır.
        /// </summary>
        /// <param name="dto">Oluşturulacak firmaya ve kullanıcıya ait veriler</param>
        /// <returns></returns>
        [HttpPost("insert")]
        [AllowAnonymous]
        public IActionResult Insert([FromBody] InsertCompanyRequestDTO dto)
        {
            InsertCompanyResponse baseResult = new InsertCompanyResponse();
            var existsCompany = unitOfWork.CompanyRepository.CheckEmail(dto.Email);
            if (existsCompany.Success)
                return NotFound(baseResult, localizer[existsCompany.Message]);
            var existsUser = unitOfWork.UserRepository.CheckEmail(dto.Email);
            if (existsUser.Success)
                return NotFound(baseResult, localizer[existsUser.Message]);

            Company company = mapper.Map<Company>(dto);
            Core.Entities.User user = mapper.Map<User>(company);
            user.UnHashedPassword = dto.Password;

            IResult companyCreated = unitOfWork.CompanyRepository.Insert(company, user);

            if (!companyCreated.Success)
                return NotFound(baseResult, localizer[companyCreated.Message]);

            unitOfWork.Commit();

            baseResult.Id = company.Id;

            return Ok(baseResult);
        }
    }
}
