using System.Data;
using Dapper;
using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;

namespace DataBaseManager.DataAccess;

public class OrderRepository : RepositoryBase, IOrderRepository
{
    public OrderRepository(IDbTransaction transaction) : base(transaction)
    {
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> GetItemsList()
    {
        return Connection.Query<Order>(
            "SELECT * FROM Orders",
            transaction: Transaction
        );
    }

    public Order GetItemById(int id)
    {
        throw new NotImplementedException();
    }

    public int Create(Order item)
    {
        throw new NotImplementedException();
    }

    public void Update(Order item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        Connection.Execute(
            "DELETE FROM Orders WHERE OrderID = @OrderID",
            param: new { OrderID = id },
            transaction: Transaction
        );
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public void SpecificOrderOnlyMethod()
    {
        throw new NotImplementedException();
    }

    public Order GetSpecificOrderBy(int idValue)
    {
        throw new NotImplementedException();
    }
}