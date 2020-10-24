using MySql.Data.MySqlClient;
using Pushenger.Core.Interfaces;
using Pushenger.Core.Utilities;
using Pushenger.Service.Repositories;
using System;
using System.Data;

namespace Pushenger.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        IDbTransaction transaction;
        IDbConnection connection;

        bool disposed;

        ICompanyRepository companyRepository;
        IUserRepository userRepository;
        IProjectRepository projectRepository;
        ITopicRepository topicRepository;
        IProjectUserRepository projectUserRepository;

        public UnitOfWork()
        {
            try
            {
                Connections connectionInfo = Connections.Instance;
                connection = new MySqlConnection(connectionInfo.MysqlConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
      
        public ICompanyRepository CompanyRepository
        {
            get
            {
                return companyRepository ?? (companyRepository = new CompanyRepository(transaction));
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return userRepository ?? (userRepository = new UserRepository(transaction));
            }
        }

        public IProjectRepository ProjectRepository {
            get
            {
                return projectRepository ?? (projectRepository = new ProjectRepository(transaction));
            }
        }

        public ITopicRepository TopicRepository
        {
            get
            {
                return topicRepository ?? (topicRepository = new TopicRepository(transaction));
            }
        }

        public IProjectUserRepository ProjectUserRepository
        {
            get
            {
                return projectUserRepository ?? (projectUserRepository = new ProjectUserRepository(transaction));
            }
        }

        public bool Commit()
        {
            bool rtn = false;
            try
            {
                transaction.Commit();
                rtn = true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                transaction = connection.BeginTransaction();
                resetRepositories();
            }
            return rtn;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Rollback()
        {
            bool rtn = false;
            try
            {
                transaction?.Rollback();
                rtn = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                transaction?.Dispose();
                transaction = connection.BeginTransaction();
                resetRepositories();
            }
            return rtn;
        }

        private void resetRepositories()
        {
            companyRepository = null;
            userRepository = null;
            projectRepository = null;
            topicRepository = null;
        }

        private void dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }

                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }
                disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
