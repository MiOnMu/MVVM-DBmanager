using DataBaseManager.DataAccess.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.AppService.Contracts
{
    public interface ISupplierService
    {

        /// <summary>
        /// Pobieranie listy wszystkich dostawców
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItemSupplierGridDTO> GetSuppliersCollection();

        /// <summary>
        /// Dodawanie nowego dostawcy
        /// </summary>
        /// <param name="inputData"></param>
        bool AddSupplier(ItemSupplierGridDTO inputData);

        /// <summary>
        /// Aktualizacja istniejącego dostawcy
        /// </summary>
        /// <param name="inputData"></param>
        void UpdateExistedSupplier(ItemSupplierGridDTO inputData);

        /// <summary>
        /// Usuwanie rekordu w bazie danych po identyfikatorze
        /// </summary>
        /// <param name="idValue"></param>
        void DeleteSupplierUsing(int idValue);
    }
}