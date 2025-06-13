using System.Data;
using Dapper;
using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;

namespace DataBaseManager.DataAccess;

public class CustomerRepository : RepositoryBase, ICustomerRepository
{

    public CustomerRepository(IDbTransaction transaction) : base(transaction)
    {
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

    public void Dispose()
    {
        // W razie potrzeby zaimplementować logikę zwalniania zasobów
        // Na przykład, możma zamknąć transakcję lub połączenie
        Transaction?.Dispose();
    }


    public IEnumerable<Customer> GetItemsList()
    {
        throw new NotImplementedException();
    }

    public Customer GetItemById(int id)
    {
        throw new NotImplementedException();
    }


    public int Create(Customer item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var sql = @"
        INSERT INTO Customers 
            (Name, ContactName, Address, City, Region, PostalCode, Country, Phone, Email)
        VALUES
            (@Name, @ContactName, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Email);
        SELECT CAST(SCOPE_IDENTITY() AS int);";

        // Przekazanie obiektu item jako parametrów — Dapper automatycznie zmapuje właściwości na nazwy parametrów
        item.Id = Connection.ExecuteScalar<int>(sql, item, transaction: Transaction);

        return item.Id; // Zwracamy wygenerowany identyfikator
    }


    // (!!!) Poniżej znajduje się przykład z ręcznym mapowaniem; gdyby nie było biblioteki Dapper, 
    // kod trzeba by było pisać w ten sposób - jest to nieporęczna konstrukcja

    //public void Create(Customer item)
    //{
    //    if (item == null)
    //        throw new ArgumentNullException(nameof(item));

    //    item.Id = Connection.ExecuteScalar<int>(
    //        @"INSERT INTO Customers 
    //    (Name, ContactName, Address, City, Region, PostalCode, Country, Phone, Email)
    //    VALUES
    //    (@Name, @ContactName, @Address, @City, @Region, @PostalCode, @Country, @Phone, @Email);
    //    SELECT SCOPE_IDENTITY();",
    //        param: new
    //        {
    //            Name        = item.Name,
    //            ContactName = item.ContactName,
    //            Address     = item.Address,
    //            City        = item.City,
    //            Region      = item.Region,
    //            PostalCode  = item.PostalCode,
    //            Country     = item.Country,
    //            Phone       = item.Phone,
    //            Email       = item.Email,
    //        },
    //        transaction: Transaction
    //    );
    //}


    public void Update(Customer item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void SpecificCustomerOnlyMethod()
    {
        throw new NotImplementedException();
    }


}