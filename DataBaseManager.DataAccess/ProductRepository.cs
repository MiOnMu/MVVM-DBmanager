using System.Data;
using Dapper;
using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;
namespace DataBaseManager.DataAccess;

public class ProductRepository : RepositoryBase, IProductRepository
{
    public ProductRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetItemsList()
    {
        return Connection.Query<Product>(
            "SELECT * FROM Products",
            transaction: Transaction
        );
    }

    public Product GetItemById(int id)
    {
        throw new NotImplementedException();
    }

    public int Create(Product item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var sql = @"
        INSERT INTO Products 
            (ProductName, SupplierID, UnitPrice, Discontinued, ProductRank, ProductSpecificData)
        VALUES
            (@ProductName, @SupplierID, @UnitPrice, @Discontinued, @ProductRank, @ProductSpecificData);
        SELECT CAST(SCOPE_IDENTITY() AS int);";

        // Przekazanie obiektu item jako parametrów — Dapper automatycznie zmapuje w³aœciwoœci na nazwy parametrów
        item.ProductID = Connection.ExecuteScalar<int>(sql, item, transaction: Transaction);

        return item.ProductID; // Zwracamy wygenerowany identyfikator
    }

    public void Update(Product item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        Connection.Execute(
            @"UPDATE Products SET 
                ProductName  = @ProductName, 
                SupplierID   = @SupplierID,
                UnitPrice    = @UnitPrice,  
                Discontinued = @Discontinued, 
                ProductRank  = @ProductRank, 
                ProductSpecificData  = @ProductSpecificData
              WHERE ProductID = @ProductID",
            param: new
            {
                ProductID = item.ProductID,
                ProductName = item.ProductName,
                SupplierID = item.SupplierID,
                UnitPrice = item.UnitPrice,
                Discontinued = item.Discontinued,
                ProductRank = item.ProductRank,
                ProductSpecificData = item.ProductSpecificData,

            },
            transaction: Transaction
        );
    }

    public void Delete(int id)
    {
        Connection.Execute(
            "DELETE FROM Products WHERE ProductID = @ProductID",
            param: new { ProductID = id },
            transaction: Transaction
        );
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void SpecificProductOnlyMethod()
    {
        throw new NotImplementedException();
    }
}