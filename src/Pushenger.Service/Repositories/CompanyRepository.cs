using NETCore.Encrypt;
using Pushenger.Core.Entities;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;
using System.Linq;

namespace Pushenger.Service.Repositories
{
    public class CompanyRepository : RepositoryBase, ICompanyRepository
    {
        public CompanyRepository(IDbTransaction transaction)
            : base(transaction)
        {
        }

        public IResult CheckEmail(string email)
        {
            Company company = connection.ExecuteCommand<Company>("SELECT * FROM company WHERE Email = @email AND Status = 1", email).FirstOrDefault();
            if (company == null)
                return new ErrorResult();
            else
                return new SuccessResult(Constant.CompanyMessages.CompanyAlreadyExists);
        }


        public IResult Insert(Company company, User user)
        {
            company.Id = connection.Insert(company);
            if (company.Id < 1)
            {
                return new ErrorResult(Constant.CompanyMessages.CompanyNotCreated);
            }
            else
            {
                user.UserTypeId = enumUserType.Owner;
                user.CompanyId = company.Id;
                user.Password = EncryptProvider.Md5(user.UnHashedPassword);
                user.Id = connection.Insert(user);
                if (user.Id < 1)
                {
                    return new ErrorResult(Constant.CompanyMessages.UserNotCreated);
                }
                else
                {
                    return new SuccessResult();
                }
            }
        }
        public IDataResult<Company> FindById(int id)
        {
            Company company = connection.ExecuteCommand<Company>("SELECT * FROM company WHERE Id = @id AND Status = 1", id).FirstOrDefault();
            if (company != null)
                return new SuccessDataResult<Company>(company);
            else
                return new ErrorDataResult<Company>(null, Constant.CompanyMessages.CompanyNotFound);            
        }

        public IResult Update(Company company)
        {
            bool isUpdated = connection.Update(company);
            if (isUpdated)
                return new SuccessResult();
            return new ErrorResult(Constant.CompanyMessages.CompanyNotUpdated);            
        }
    }
}
