using DataBaseManager.DataAccess.Contracts;
using System.Data;
using System.Data.SqlClient;

namespace DataBaseManager.DataAccess;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private IDbConnection _connection;
    private IDbTransaction _transaction;
    private ICustomerRepository _customerRepository;
    private ISupplierRepository _supplierRepository;
    private IEventsHistoryRepository _eventsHistoryRepository;
    private bool _disposed;

    public UnitOfWork(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }

    /// <summary>
    /// Dostęp do repozytorium zdarzeń systemowych
    /// </summary>
    public IEventsHistoryRepository EventsRepository
        => _eventsHistoryRepository ??= new EventsHistoryRepository(_transaction);

    /// <summary>
    /// Dostęp do repozytorium klientów
    /// </summary>
    public ICustomerRepository CustomerRepository
        => _customerRepository ??= new CustomerRepository(_transaction);

    /// <summary>
    /// Dostęp do repozytorium dostawców
    /// </summary>>
    public ISupplierRepository SupplierRepository
        => _supplierRepository ??= new SupplierRepository(_transaction);


    public void Commit()
    {
        try
        {
            _transaction.Commit();
        }
        catch
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
    }

    private void resetRepositories()
    {
        _customerRepository      = null;
        _supplierRepository      = null;
        _eventsHistoryRepository = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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
        Dispose(false);
    }

    private void ReleaseUnmanagedResources()
    {
        // TODO zwolnij tutaj niezarządzane zasoby
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _connection.Dispose();
            _transaction.Dispose();
            _customerRepository?.Dispose();
            _supplierRepository?.Dispose();
        }
    }
}