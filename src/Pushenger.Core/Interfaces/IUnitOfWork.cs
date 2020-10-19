using System;

namespace Pushenger.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository CompanyRepository { get; }
        IUserRepository UserRepository { get; }
        bool Commit();
        bool Rollback();
    }
}
