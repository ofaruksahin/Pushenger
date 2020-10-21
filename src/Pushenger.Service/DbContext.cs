using Dapper;
using Dapper.Contrib.Extensions;
using Pushenger.Core.Entities;
using Pushenger.Core.Enums;
using Pushenger.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Pushenger.Service
{
    public class DbContext : IDbContext
    {
        readonly IDbTransaction _transaction;

        IDbConnection _connection
        {
            get
            {
                return _transaction.Connection;
            }
        }

        public DbContext(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public T GetByID<T>(long id) where T : class, new()
        {
            try
            {
                return _connection.Get<T>(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public IEnumerable<T> GetList<T>() where T : class, new()
        {
            try
            {
                return _connection.GetAll<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public int Insert<T>(T item) where T : class, new()
        {            
            try
            {
                return (int)_connection.Insert(item, _transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public bool Update<T>(T item) where T : BaseEntity, new()
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                return _connection.Update(item, _transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public bool Delete<T>(T item) where T : BaseEntity, new()
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                item.Status = enumRecordStatus.InActive;
                return _connection.Update(item, _transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public IEnumerable<T> ExecuteCommand<T>(string sql, params object[] args)
        {
            try
            {
                return _connection.Query<T>(sql, CreateParams(GetMethodName(), args));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public IEnumerable<T> ExecuteCommand<T>(string sql)
        {
            try
            {
                return _connection.Query<T>(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public IEnumerable<T> ExecuteProcedure<T>(string sql, params object[] args)
        {
            try
            {
                return _connection.Query<T>(sql, CreateParams(GetMethodName(), args), null, true, null, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public MethodInfo GetMethodName()
        {
            var trace = new StackTrace();
            return (MethodInfo)trace.GetFrame(2).GetMethod();
        }

        public DynamicParameters CreateParams(MethodInfo _Method, params object[] _Values)
        {
            DynamicParameters sqlParams = new DynamicParameters();
            int paramIndex = 0;
            ParameterInfo[] methodParameters = _Method.GetParameters();
            foreach (ParameterInfo paramInfo in methodParameters)
            {
                sqlParams.Add(paramInfo.Name, _Values[paramIndex]);
                paramIndex++;
            }
            return sqlParams;
        }
    }
}
