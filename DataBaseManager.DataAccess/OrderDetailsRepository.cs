using Dapper;
using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;
using System.Data;

namespace DataBaseManager.DataAccess;

public class OrderDetailsRepository : RepositoryBase, IOrderDetailsRepository
{
    public OrderDetailsRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OrderDetails> GetItemsList()
    {
        return Connection.Query<OrderDetails>(
            "SELECT * FROM OrderDetails",
            transaction: Transaction
        );
    }

    public OrderDetails GetItemById(int id)
    {
        throw new NotImplementedException();
    }

    public int Create(OrderDetails item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        var sql = @"
        INSERT INTO OrderDetails 
            (OrderID, ProductID, UnitPrice, Quantity, Discount, ProductName)
        VALUES
            (@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount, @ProductName);
        SELECT CAST(SCOPE_IDENTITY() AS int);";

        // Przekazanie obiektu item jako parametrów — Dapper automatycznie zmapuje właściwości na nazwy parametrów
        item.ProductID = Connection.ExecuteScalar<int>(sql, item, transaction: Transaction);

        return item.ProductID; // Zwracamy wygenerowany identyfikator
    }

    public void Update(OrderDetails item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        Connection.Execute(
            "DELETE FROM OrderDetails WHERE OrderDetailID = @OrderDetailID",
            param: new { OrderDetailID = id },
            transaction: Transaction
        );
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void SpecificOrderDetailsOnlyMethod()
    {
        throw new NotImplementedException();
    }
}