using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;

namespace DataBaseManager.AppService.Contracts;

public interface IProductService
{
    IEnumerable<ItemDTO> GetSuppliersCollection();

    IEnumerable<ItemProductGridDTO> GetProductsCollectionUsing(int idValue);

    bool CreateNewProductUsingMapped(ItemProductGridDTO product);

    void DeleteProductUsingMapped(int idValue);
    void UpdateProductUsingMapped(ItemProductGridDTO product);

}                                                                                