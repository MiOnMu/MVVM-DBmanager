using DataBaseManager.Core;
using DataBaseManager.Core.Models;

namespace DataBaseManager.DataAccess.Contracts;

public interface ICustomerRepository : IRepository<Customers>
{
    void SpecificCustomerOnlyMethod();
}