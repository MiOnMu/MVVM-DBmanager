using DataBaseManager.Core.Models;
using DataBaseManager.DataAccess.Contracts;

namespace DataBaseManager.DataAccess;

public class CustomerRepository : ICustomerRepository
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Customers> GetBookList()
    {
        throw new NotImplementedException();
    }

    public Customers GetBook(int id)
    {
        throw new NotImplementedException();
    }

    public void Create(Customers item)
    {
        throw new NotImplementedException();
    }

    public void Update(Customers item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
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