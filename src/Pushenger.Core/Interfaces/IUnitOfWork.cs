using System;

namespace Pushenger.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        bool Rollback();
    }
}
