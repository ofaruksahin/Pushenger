using System;

namespace Pushenger.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository CompanyRepository { get; }
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ITopicRepository TopicRepository { get; }
        IProjectUserRepository ProjectUserRepository { get; }
        ISubscriptionRepository SubscriptionRepository { get; }
        INotificationRepository NotificationRepository { get; }
        bool Commit();
        bool Rollback();
    }
}
