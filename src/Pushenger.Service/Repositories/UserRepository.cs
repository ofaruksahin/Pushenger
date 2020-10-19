using Pushenger.Core.Entities;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System.Data;
using System.Linq;

namespace Pushenger.Service.Repositories
{
    public class UserRepository : RepositoryBase,IUserRepository
    {
        public UserRepository(IDbTransaction transaction) 
            : base(transaction)
        {
        }

        public IResult CheckEmail(string email)
        {
            User user = connection.ExecuteCommand<User>("SELECT * FROM user WHERE Email = @email AND Status = 1", email).FirstOrDefault();
            if (user == null)
                return new ErrorResult();
            else
                return new SuccessResult(Constant.CompanyMessages.UserAlreadyExists);
        }
    }
}
