using DataBaseManager.AppService.Contracts;
using DataBaseManager.Core.Models;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess;
using DataBaseManager.DataAccess.Contracts;
using DataBaseManager.DataAccess.Contracts.DTOs;
using Mapster;

namespace DataBaseManager.AppService;

/// <summary>
/// Klasa serwisu aplikacji
/// </summary>
public class DbAppService : IDbAppService
{
    string connectionString =
        "server=COMP; database=DB; Integrated Security=true; TrustServerCertificate=True;";

    #region SPECYFICZNE DLA KLIENTA

    /// <summary>
    /// Implementacja metody pobierającej listę użytkowników
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<ItemCustomerGridDTO> GetCustomersCollection()
    {
        // Dopóki część związana z bazą danych jest jeszcze w fazie rozwoju,
        // dostarczamy dane tymczasowe (zaślepkowe).
        // Jest to normalna i powszechna praktyka,
        // aby nie wstrzymywać pracy grupy programistów, która
        // zajmuje się warstwą prezentacji (Presentation Layer).

        // Oto te dane tymczasowe
        List<ItemCustomerGridDTO> retCustomers = new List<ItemCustomerGridDTO>();
        retCustomers.Add(new ItemCustomerGridDTO
        {
            Id = 1,
            Name = "Name 1",
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
            Name = "Name 2",
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
            Name = "Name 3",
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

    /// <summary>
    /// Główna metoda dodawania nowego klienta do bazy danych.
    /// Odnosi się do metody ICustomerService.AddCustomer.
    /// Znajduje się w warstwie logiki biznesowej aplikacji.
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    public bool AddCustomer(ItemCustomerGridDTO inputData)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                // Tworzymy nowy obiekt Customer z DTO
                var customer = inputData.Adapt<Customer>();
                var created = unitOfWork.CustomerRepository.Create(customer);

                // Dodajemy wpis do historii zdarzeń
                unitOfWork.EventsRepository
                    .AddEventHistory($"New client adding {inputData.Name}");

                // Zapisujemy zmiany w bazie danych
                unitOfWork.Commit();

                // Jeśli metoda Create zwraca obiekt lub bool, można sprawdzić wynik.
                // Tutaj zakładamy, że jeśli nie wystąpił wyjątek, wszystko przebiegło pomyślnie.
                return true;
            }
        }
        #region Blok Catch
        catch (Exception ex)
        {
            // Tutaj można dodać logowanie błędu, jeśli jest to wymagane.
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania klienta");
            return false;
        }
        #endregion
    }

    public void UpdateExistedCustomer(ItemCustomerGridDTO inputData)
    {
        // Tutaj znajdzie się kod, który wywoła repozytorium Customer w celu AKTUALIZACJI
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