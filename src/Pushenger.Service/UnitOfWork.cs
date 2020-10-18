using MySql.Data.MySqlClient;
using Pushenger.Core.Interfaces;
using System;
using System.Data;

namespace Pushenger.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        IDbTransaction _transaction;
        IDbConnection _connection;

        bool _disposed;

        public UnitOfWork()
        {
            try
            {
                _connection = new MySqlConnection("");
                _connection.Open();
                _transaction = _connection.BeginTransaction();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
      

        public bool Commit()
        {
            bool rtn = false;
            try
            {
                _transaction.Commit();
                rtn = true;
            }
            catch (Exception)
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
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
                _transaction?.Rollback();
                rtn = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
            return rtn;
        }

        private void resetRepositories()
        {
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
