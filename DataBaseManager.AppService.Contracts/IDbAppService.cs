using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts;

namespace DataBaseManager.AppService.Contracts;

/// <summary>
/// Już nie pusty kontrakt dla serwisu aplikacji
/// </summary>
public interface IDbAppService : ICustomerService, ISupplierService
{
    /// <summary>
    /// Jakaś na razie nieokreślona aktywność, ale charakterystyczna
    /// ogólnie dla serwisu aplikacji, a nie tylko na przykład
    /// dla Klienta (Customer)
    /// </summary>
    void GeneralServiceAction();
}