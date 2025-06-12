using DataBaseManager.AppService.Contracts;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts;
using DataBaseManager.DataAccess.Contracts.DTOs;

namespace DataBaseManager.AppService;

/// <summary>
/// Klasa serwisu aplikacji
/// </summary>
public class DbAppService : IDbAppService
{

    #region CUSTOMER SPECIFIC

    /// <summary>
    /// Implementacja metody pobierającej listę klientów
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<ItemCustomerGridDTO> GetCustomersCollection()
    {
        // Ponieważ część dotycząca bazy danych jest jeszcze w fazie rozwoju,
        // udostępniamy fikcyjne dane.
        // Jest to normalna i powszechna praktyka,
        // aby nie zatrzymywać pracy tej grupy programistów, która
        // zajmuje się warstwą prezentacji (Presentation Layer).


        // To są fikcyjne dane
        List<ItemCustomerGridDTO> retCustomers = new List<ItemCustomerGridDTO>();
        retCustomers.Add(new ItemCustomerGridDTO
        {
            Id = 1,
            ContactName = "Customer 1",
            Phone = "111-111-111-111",
            Email = "1@1.com",
            Address = "Address 1",
            City = "City 1",
            Country = "Country 1",
            PostalCode = "Post Code 1",
            Region = "Region 1"

        });

        retCustomers.Add(new ItemCustomerGridDTO
        {
            Id = 2,
            ContactName = "Customer 2",
            Phone = "222-222-222-222",
            Email = "2@2.com",
            Address = "Address 2",
            City = "City 2",
            Country = "Country 2",
            PostalCode = "Post Code 2",
            Region = "Region 2"

        });

        retCustomers.Add(new ItemCustomerGridDTO
        {
            Id = 3,
            ContactName = "Customer 3",
            Phone = "333-333-333-333",
            Email = "3@3.com",
            Address = "Address 3",
            City = "City 3",
            Country = "Country 3",
            PostalCode = "Post Code 3",
            Region = "Region 3"

        });

        return retCustomers;
    }

    public void AddCustomer(ItemCustomerGridDTO inputData)
    {
        // Tutaj znajdzie się kod, który wywoła repozytorium Klientów w celu ZAPISU
    }

    public void UpdateExistedCustomer(ItemCustomerGridDTO inputData)
    {
        // Tutaj znajdzie się kod, który wywoła repozytorium Klientów w celu AKTUALIZACJI
    }

    public void DeleteCustomerUsing(int idValue)
    {
        throw new NotImplementedException();
    }

    #endregion

    public void GeneralServiceAction()
    {
        throw new NotImplementedException();
    }
}