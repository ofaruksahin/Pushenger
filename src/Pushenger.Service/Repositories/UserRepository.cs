using NETCore.Encrypt;
using Newtonsoft.Json;
using Pushenger.Core.Entities;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Core.Utilities.Result;
using System;
using System.Data;
using System.Linq;

namespace Pushenger.Service.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
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

        public IDataResult<User> FindByUser(string email, string password)
        {
            password = EncryptProvider.Md5(password);
            User user = connection.ExecuteCommand<User>(@"SELECT 
                Id,
                UserTypeId,
                CompanyId,
                Name,
                Surname,
                Email,
                CreationDate,
                ModifiedDate,
                CreatorId,
                Status
            FROM user WHERE Email = @email AND Password = @password AND Status = 1", email, password).FirstOrDefault();
            if (user != null)
                return new SuccessDataResult<User>(user);
            else
                return new ErrorDataResult<User>(null, Constant.UserMessages.UserNotFound);
        }

        public IDataResult<string> LogIn(User user)
        {
            string token = JWTManager.GenerateToken(user);
            try
            {
                cache.GetDatabase((int)enumRedisDatabase.auth).StringSet(token, JsonConvert.SerializeObject(token));
                return new SuccessDataResult<string>(token);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>("", Constant.UserMessages.LoginError);   
            }            
        }
    }
}
