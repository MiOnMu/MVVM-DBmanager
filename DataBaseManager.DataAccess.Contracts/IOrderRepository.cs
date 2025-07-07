using DataBaseManager.Core.Models;
using DataBaseManager.Core;

namespace DataBaseManager.DataAccess.Contracts;

public interface IOrderRepository : IRepository<Order>
{
    void SpecificOrderOnlyMethod();

    Order GetSpecificOrderBy(int idValue);
}