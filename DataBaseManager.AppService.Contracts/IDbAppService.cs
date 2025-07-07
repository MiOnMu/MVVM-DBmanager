using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts;

namespace DataBaseManager.AppService.Contracts;

public interface IDbAppService :
    ICustomerService,
    ISupplierService,
    IProductService,
    IOrderService
{
    /// <summary>
    /// Jakaś na razie nieokreślona aktywność, ale charakterystyczna
    /// ogólnie dla serwisu aplikacji
    /// </summary>
    void GeneralServiceAction();
}