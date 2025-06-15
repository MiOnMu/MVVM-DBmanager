using Dapper;
using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;
using System.Data;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace DataBaseManager.DataAccess;

public class CustomerRepository : RepositoryBase, ICustomerRepository
{

    public CustomerRepository(IDbTransaction transaction) : base(transaction)
    {
    }
        
    public void Dispose()
    {
        // W razie potrzeby zaimplementuj logikę zwalniania zasobów
        // Na przykład, możesz chcieć zamknąć transakcję lub połączenie
        Transaction?.Dispose();
    }


    public IEnumerable<Customer> GetItemsList()
    {
        return Connection.Query<Customer>(
                "SELECT * FROM Customers",
                transaction: Transaction
            );
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
        item.CustomerId = Connection.ExecuteScalar<int>(sql, item, transaction: Transaction);

        return item.CustomerId; // Zwracamy wygenerowany identyfikator
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

    /// <summary>
    /// Aktualizacja danych klienta w bazie danych
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Update(Customer item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        Connection.Execute(
            @"UPDATE Customers SET 
                Name        = @Name, 
                ContactName = @ContactName, 
                Address     = @Address,  
                City        = @City, 
                Region      = @Region,
                PostalCode  = @PostalCode,  
                Country     = @Country, 
                Phone       = @Phone, 
                Email       = @Email
              WHERE CustomerID = @CustomerID",
            param: new
            {
                CustomerID = item.CustomerId,
                Name = item.Name,
                ContactName = item.ContactName,
                Address = item.Address,
                City = item.City,
                Region = item.Region,
                PostalCode = item.PostalCode,
                Country = item.Country,
                Phone = item.Phone,
                Email = item.Email
            },
            transaction: Transaction
        );
    }


    /// <summary>
    /// Usuwanie pojedynczego klienta po identyfikatorze
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        Connection.Execute(
            "DELETE FROM Customers WHERE CustomerID = @CustomerID",
            param: new { CustomerID = id },
            transaction: Transaction
        );
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