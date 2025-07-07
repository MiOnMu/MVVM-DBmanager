using DataBaseManager.Core.Models;
using DataBaseManager.Core;

namespace DataBaseManager.DataAccess.Contracts;

public interface IOrderDetailsRepository : IRepository<OrderDetails>
{
    void SpecificOrderDetailsOnlyMethod();
}