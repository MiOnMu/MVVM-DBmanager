using Dapper;
using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;
using System.Data;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace DataBaseManager.DataAccess;

public class SupplierRepository : RepositoryBase, ISupplierRepository
{

    public SupplierRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public void Dispose()
    {
        // W razie potrzeby zaimplementuj logikę zwalniania zasobów
        // Na przykład, możesz chcieć zamknąć transakcję lub połączenie
        Transaction?.Dispose();
    }


    public IEnumerable<Supplier> GetItemsList()
    {
        return Connection.Query<Supplier>(
                "SELECT * FROM Suppliers",
                transaction: Transaction
            );
    }

    public Supplier GetItemById(int id)
    {
        throw new NotImplementedException();
    }


    public int Create(Supplier item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var sql = @"
        INSERT INTO Suppliers 
            (CompanyName, ContactName, Phone, Email)
        VALUES
            (@Name, @ContactName, @Phone, @Email);
        SELECT CAST(SCOPE_IDENTITY() AS int);";

        // Przekazanie obiektu item jako parametrów — Dapper automatycznie zmapuje właściwości na nazwy parametrów
        item.SupplierId = Connection.ExecuteScalar<int>(sql, item, transaction: Transaction);

        return item.SupplierId; // Zwracamy wygenerowany identyfikator
    }

    public void Update(Supplier item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        Connection.Execute(
            @"UPDATE Suppliers SET 
                CompanyName = @Name, 
                ContactName = @ContactName, 
                Phone       = @Phone, 
                Email       = @Email
              WHERE SupplierID = @SupplierID",
            param: new
            {
                SupplierID = item.SupplierId,
                CompanyName = item.CompanyName,
                ContactName = item.ContactName,
                Phone = item.Phone,
                Email = item.Email
            },
            transaction: Transaction
        );
    }


    /// <summary>
    /// Usuwanie pojedynczego dostawcy po identyfikatorze
    /// </summary>
    /// <param name="id"></param>
    public void Delete(int id)
    {
        Connection.Execute(
            "DELETE FROM Suppliers WHERE SupplierID = @SupplierID",
            param: new { SupplierID = id },
            transaction: Transaction
        );
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void SpecificSupplierOnlyMethod()
    {
        throw new NotImplementedException();
    }


}