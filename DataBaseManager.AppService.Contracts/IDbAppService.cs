using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;

namespace DataBaseManager.AppService.Contracts;

/// <summary>
/// Już nie pusty kontrakt dla serwisu aplikacji
/// </summary>
public interface IDbAppService
{
    /// <summary>
    /// Pobieranie pełnej kolekcji klientów
    /// </summary>
    /// <returns></returns>
    IEnumerable<ItemCustomerGridDTO> GetCustomersCollection();
}