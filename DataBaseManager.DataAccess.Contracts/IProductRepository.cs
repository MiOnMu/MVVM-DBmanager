using DataBaseManager.Core.Models;
using DataBaseManager.Core;

namespace DataBaseManager.DataAccess.Contracts;

public interface IProductRepository : IRepository<Product>
{
    void SpecificProductOnlyMethod();
}