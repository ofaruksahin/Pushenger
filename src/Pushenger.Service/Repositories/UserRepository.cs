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
                cache.GetDatabase((int)enumRedisDatabase.auth).StringSet(token, JsonConvert.SerializeObject(user));
                return new SuccessDataResult<string>(token);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>("", Constant.UserMessages.LoginError);   
            }            
        }

        public IDataResult<User> CheckToken(string token)
        {
            try
            {
                var db =  cache.GetDatabase((int)enumRedisDatabase.auth);
                bool exist = db.KeyExists(token);
                if (!exist)
                    return new ErrorDataResult<User>(null,Constant.TokenNotFound);
                var value = db.StringGet(token);
                return new SuccessDataResult<User>(JsonConvert.DeserializeObject<User>(value));
            }
            catch (Exception)
            {
                return new ErrorDataResult<User>(null, Constant.TokenNotFound);                
            }            
        }

        public IDataResult<User> GetUser(int id)
        {
            User user = connection.ExecuteCommand<User>("SELECT * FROM user WHERE Id = @id AND Status = 1", id).FirstOrDefault();
            if (user == null)
                return new ErrorDataResult<User>(null);
            return new SuccessDataResult<User>(user);
        }

        public IResult UpdateUser(User user)
        {
            if (!String.IsNullOrEmpty(user.UnHashedPassword))
                user.Password = EncryptProvider.Md5(user.UnHashedPassword);
            bool isUpdated = connection.Update(user);
            if (isUpdated)
                return new SuccessResult();
            return new ErrorResult(Constant.UserMessages.UpdateError);
        }

        public IResult LogOutUser(string token)
        {
            cache.GetDatabase((int)enumRedisDatabase.auth).KeyDelete(token);
            return new SuccessResult(Constant.LogOuted);
        }

        public IResult Insert(User user)
        {
            user.Password = EncryptProvider.Md5(user.UnHashedPassword);
            user.Id = connection.Insert(user);
            if (user.Id < 1)
                return new ErrorResult();
            return new SuccessResult();            
        }

        public IResult Delete(User user)
        {
            bool isDeleted = connection.Delete(user);
            if (isDeleted)
                return new SuccessResult();
            return new ErrorResult(Constant.UserMessages.UserNotDeleted);
        }
    }
}
