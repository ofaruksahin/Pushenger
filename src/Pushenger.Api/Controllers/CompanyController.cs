using AutoMapper;
using Microsoft.Extensions.Localization;
using Pushenger.Core.Interfaces;

namespace Pushenger.Api.Controllers
{
    public class CompanyController 
        : BaseController
    {
        IStringLocalizer<CompanyResource> localizer;

        public CompanyController(
            IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IStringLocalizer<CompanyResource> _localizer
            ) 
            : base(_unitOfWork, _mapper)
        {
            localizer = _localizer;
        }
    }
}
