using DataBaseManager.Core;
using DataBaseManager.Core.Models;

namespace DataBaseManager.DataAccess.Contracts;

public interface ISupplierRepository : IRepository<Supplier>
{
    void SpecificSupplierOnlyMethod();
}