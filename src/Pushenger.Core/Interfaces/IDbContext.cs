using Pushenger.Core.Entities;
using System.Collections.Generic;

namespace Pushenger.Core.Interfaces
{
    public interface IDbContext
    {
        T GetByID<T>(long id) where T : class, new();
        IEnumerable<T> GetList<T>() where T : class, new();
        int Insert<T>(T item) where T : class, new();
        bool Update<T>(T item) where T : BaseEntity, new();
        bool Delete<T>(T item) where T : BaseEntity, new();
        IEnumerable<T> ExecuteCommand<T>(string sql, params object[] args);
        IEnumerable<T> ExecuteCommand<T>(string sql);
        IEnumerable<T> ExecuteProcedure<T>(string sql, params object[] args);
    }
}
