using DataBaseManager.AppService.Contracts;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;

namespace DataBaseManager.AppService;

/// <summary>
/// Już nie pusta klasa, tutaj znajdą się serwisy aplikacji w rzeczywistej aplikacji
/// </summary>
public class DbAppService : IDbAppService
{
    /// <summary>
    /// Implementacja metody pobierania listy klientów
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<ItemCustomerGridDTO> GetCustomersCollection()
    {
        // Dopóki część związana z bazą danych jest jeszcze w fazie rozwoju,
        // dostarczamy dane fikcyjne.
        // Jest to normalna i powszechna praktyka,
        // aby nie wstrzymywać pracy grupy programistów, która
        // zajmuje się warstwą prezentacji (Presentation Layer).


        // Oto te fikcyjne dane
        List<ItemCustomerGridDTO> retCustomers = new List<ItemCustomerGridDTO>();
        retCustomers.Add(new ItemCustomerGridDTO
        {
            Id = 1,
            ContactName = "Customer 1",
            CustomerPhone = "111-111-111-111",
            CustomerEmail = "1@1.com"

        });

        retCustomers.Add(new ItemCustomerGridDTO
        {
            Id = 2,
            ContactName = "Customer 2",
            CustomerPhone = "222-222-222-222",
            CustomerEmail = "2@2.com"

        });

        retCustomers.Add(new ItemCustomerGridDTO
        {
            Id = 3,
            ContactName = "Customer 3",
            CustomerPhone = "333-333-333-333",
            CustomerEmail = "3@3.com"

        });



        return retCustomers;
    }
}