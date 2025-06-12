using DataBaseManager.DataAccess.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.AppService.Contracts
{
    public interface ICustomerService
    {

        /// <summary>
        /// Pobieranie listy wszystkich klientów
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItemCustomerGridDTO> GetCustomersCollection();

        /// <summary>
        /// Dodawanie nowego klienta
        /// </summary>
        /// <param name="inputData"></param>
        void AddCustomer(ItemCustomerGridDTO inputData);

        /// <summary>
        /// Aktualizacja istniejącego klienta
        /// </summary>
        /// <param name="inputData"></param>
        void UpdateExistedCustomer(ItemCustomerGridDTO inputData);

        /// <summary>
        /// Usuwanie rekordu w bazie danych po identyfikatorze
        /// </summary>
        /// <param name="idValue"></param>
        void DeleteCustomerUsing(int idValue);
    }
}