using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;

namespace DataBaseManager.AppService.Contracts;

public interface IOrderService
{
    /// <summary>
    /// Pobieranie tylko krótkiej listy użytkowników
    /// </summary>
    /// <returns></returns>
    IEnumerable<ItemDTO> GetCustomersCollection();

    /// <summary>
    /// Pobieranie tylko krótkiej listy produktów
    /// </summary>
    /// <returns></returns>
    IEnumerable<ItemDTO> GetProductsCollection();

    /// <summary>
    /// Pobieranie tylko nagłówków zamówień
    /// </summary>
    /// <param name="idValue"></param>
    /// <returns></returns>
    IEnumerable<ItemOrderGridDTO> GetOrdersCollectionUsing(int idValue);

    /// <summary>
    /// Pobieranie części tabelarycznej zamówień
    /// </summary>
    /// <param name="idValue"></param>
    /// <returns></returns>
    IEnumerable<ItemOrderDetailsGridDTO> GetOrdersTabCollectionUsing(int idValue);

    /// <summary>
    /// Usuwanie zamówienia i kaskadowo powiązanych z nim danych
    /// </summary>
    /// <param name="idValue"></param>
    void DeleteOrderUsingMapped(int idValue);

    /// <summary>
    /// Usuwanie elementu tabelarycznego zamówienia
    /// </summary>
    /// <param name="idValue"></param>
    void DeleteOrderTabItemUsing(int idValue);

    /// <summary>
    /// Główna metoda wpływająca na PONOWNE OKREŚLENIE buforowanego obiektu
    /// dokumentu zamówienia
    /// </summary>
    /// <param name="idValue">Identyfikator w tabeli Orders</param>
    /// <returns></returns>
    bool DocumentRedefinitionUsing(int idValue);

    /// <summary>
    /// Ustawienie nowego elementu tabeli
    /// w obiekcie zamówienia
    /// </summary>
    /// <param name="itemTab"></param>
    void SetTabItemUsing(ItemOrderDetailsGridDTO itemTab);
}