using System.Globalization;
using DataBaseManager.AppService.Contracts;
using DataBaseManager.Core.Aggregates;
using DataBaseManager.Core.Models;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess;
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

    #region Ctors
    public DbAppService()
    { DocumentModel = new DocumentOrderAggregate(); }
    #endregion

    #region LOGON SPECIFIC
    public bool IsUserValid(string passEncryptedData, string userName,
        out bool isUserAdmin)
    {
        bool retResult = false;  
        isUserAdmin = false;
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {

                SysUser sysUser = unitOfWork.LogonInfoRepository
                    .GetUserInfoByPassword(passEncryptedData, userName, out isUserAdmin);

                if (sysUser.IsCorrectRetrieved)
                {
                    unitOfWork.EventsRepository
                        .AddEventHistory($"Attempt to retrieve object SysUser with name {sysUser.UserName} and  ID: {sysUser.UserID}");

                    unitOfWork.Commit();
                    retResult = true;
                }
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane

        }
        #endregion

        return retResult;
    }

    public bool AddNewUser(string passData, string userName)
    {
        bool retResult = false;
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {

                unitOfWork.LogonInfoRepository.CreateNewUser(passData, userName);

                unitOfWork.EventsRepository
                    .AddEventHistory($"Attempt to add object SysUser with user name {userName}");

                unitOfWork.Commit();

                retResult = true;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
        }
        #endregion

        return retResult;
    }

    #endregion

    #region CACHED DOCUMENT OBJECT 
    public DocumentOrderAggregate DocumentModel { get; private set; }
    #endregion

    #region CUSTOMER SPECIFIC

    /// <summary>
    /// Implementacja metody pobierania listy klientów
    /// Zwraca rzeczywiste informacje o klientach, a nie dane testowe
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IEnumerable<ItemCustomerGridDTO> GetCustomersCollection()
    {
        List<ItemCustomerGridDTO> retCustomers = new List<ItemCustomerGridDTO>();

        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {

                // Pobieramy listę klientów z repozytorium
                // W tym miejscu otrzymujemy surowe MODELE, a nie DTO
                IEnumerable<Customer> created = unitOfWork.CustomerRepository.GetItemsList();


                #region Sekcja Mapowania

                // Jednocześnie kontrakt serwisu wymaga zwracania właśnie DTO
                // ponieważ do warstwy prezentacji przekazujemy DTO, a nie modele domenowe (!!!)
                // Zrobiono to w celu oddzielenia logiki biznesowej od prezentacji i uniknięcia przesyłania zbędnych danych
                // oraz zależności między warstwami aplikacji.
                // Ponadto modele mogą zawierać informacje poufne, których nie należy przekazywać do części klienckiej
                // Dlatego właśnie konwertujemy otrzymane modele na DTO
                // Używamy Mapstera do konwersji modeli na DTO
                // aby nie mapować tego wszystkiego ręcznie; jest to nieporęczne, męczące i nieefektywne
                foreach (var customer in created)
                {

                    // Konwertujemy każdy model Customer na DTO ItemCustomerGridDTO
                    var customerDto = customer.Adapt<ItemCustomerGridDTO>(); // Używamy metody Adapt z Mapstera do mapowania
                    retCustomers.Add(customerDto);                           // Dodajemy DTO do listy wynikowej
                }
                #endregion

                // Dodajemy wpis do historii zdarzeń
                unitOfWork.EventsRepository
                    .AddEventHistory($"Pobrano {retCustomers.Count} klientów");

                // Zapisujemy zmiany w bazie danych
                unitOfWork.Commit();

                // Jeśli Create zwraca obiekt lub bool, można sprawdzić wynik
                // Tutaj zakłada się, że jeśli nie wystąpił wyjątek — wszystko zakończyło się pomyślnie
                return retCustomers;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania klienta");
        }
        #endregion



        return retCustomers;
    }

    /// <summary>
    /// Główna metoda dodawania nowego klienta do bazy danych
    /// Odnosi się do metody ICustomerService.AddCustomer
    /// Znajduje się w warstwie logiki biznesowej aplikacji
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
                    .AddEventHistory($"Dodawanie nowego klienta {inputData.Name}");

                // Zapisujemy zmiany w bazie danych
                unitOfWork.Commit();

                // Jeśli Create zwraca obiekt lub bool, można sprawdzić wynik
                // Tutaj zakłada się, że jeśli nie wystąpił wyjątek — wszystko zakończyło się pomyślnie
                return true;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania klienta");
            return false;
        }
        #endregion
    }

    public void UpdateExistedCustomer(ItemCustomerGridDTO inputData)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                // Tworzymy nowy obiekt Customer z DTO
                var customer = inputData.Adapt<Customer>();
                unitOfWork.CustomerRepository.Update(customer);

                // Dodajemy wpis o aktualizacji do historii zdarzeń
                unitOfWork.EventsRepository
                    .AddEventHistory($"Aktualizacja klienta {inputData.Name} o identyfikatorze {inputData.CustomerId}");

                // Zapisujemy zmiany w bazie danych
                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania klienta");
        }
        #endregion
    }

    public void DeleteCustomerUsing(int idValue)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                unitOfWork.CustomerRepository.Delete(idValue);

                // Dodajemy wpis o usunięciu do historii zdarzeń
                unitOfWork.EventsRepository
                    .AddEventHistory($"Usunięcie klienta o identyfikatorze {idValue}");

                // Zapisujemy zmiany w bazie danych
                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania klienta");
        }
        #endregion
    }

    #endregion

    #region SUPPLIER SPECIFIC
    public IEnumerable<ItemSupplierGridDTO> GetSuppliersCollection()
    {
        List<ItemSupplierGridDTO> retSuppliers = new List<ItemSupplierGridDTO>();

        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                IEnumerable<Supplier> created = unitOfWork.SupplierRepository.GetItemsList();


                #region Sekcja Mapowania
                foreach (var supplier in created)
                {
                    var supplierDto = supplier.Adapt<ItemSupplierGridDTO>();
                    retSuppliers.Add(supplierDto);
                }
                #endregion

                unitOfWork.EventsRepository
                    .AddEventHistory($"Pobrano {retSuppliers.Count} dostawców");

                unitOfWork.Commit();

                return retSuppliers;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion



        return retSuppliers;
    }

    /// <summary>
    /// Główna metoda dodawania nowego dostawcy do bazy danych
    public bool AddSupplier(ItemSupplierGridDTO inputData)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                var supplier = inputData.Adapt<Supplier>();
                var created = unitOfWork.SupplierRepository.Create(supplier);

                unitOfWork.EventsRepository
                    .AddEventHistory($"Dodawanie nowego dostawcy {inputData.CompanyName}");

                unitOfWork.Commit();

                return true;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            return false;
        }
        #endregion
    }

    public void UpdateExistedSupplier(ItemSupplierGridDTO inputData)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                var supplier = inputData.Adapt<Supplier>();
                unitOfWork.SupplierRepository.Update(supplier);

                unitOfWork.EventsRepository
                    .AddEventHistory($"Aktualizacja dostawcy {inputData.CompanyName} o identyfikatorze {inputData.SupplierId}");

                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
        }
        #endregion
    }

    public void DeleteSupplierUsing(int idValue)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                unitOfWork.SupplierRepository.Delete(idValue);

                unitOfWork.EventsRepository
                    .AddEventHistory($"Usunięcie klienta o identyfikatorze {idValue}");

                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
        }
        #endregion
    }

    #endregion

    #region PRODUCT SPECIFIC

    public bool CreateNewProductUsingMapped(ItemProductGridDTO inputData)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                // Tworzymy nowy obiekt Product z DTO
                var product = inputData.Adapt<Product>();

                // Dodatkowe działania w celu przypisania
                // domyślnych danych do pól takich jak
                // ProductRank i ProductSpecificData

                int createdId = unitOfWork.ProductRepository.Create(product);
                if (createdId <= 0)
                    throw new Exception("error creation new product");

                // Dodajemy wpis do historii zdarzeń
                unitOfWork.EventsRepository.AddEventHistory($"Dodawanie nowego produkta {inputData.ProductName}");

                // Zapisujemy zmiany w bazie danych
                unitOfWork.Commit();
                return true;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania klienta");
            return false;
        }
        #endregion
    }

    public void DeleteProductUsingMapped(int idValue)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                unitOfWork.ProductRepository.Delete(idValue);

                unitOfWork.EventsRepository.AddEventHistory($"Usunięcie produkta o identyfikatorze {idValue}");

                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
        }
        #endregion
    }

    public void UpdateProductUsingMapped(ItemProductGridDTO inputData)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                Product oProduct = inputData.Adapt<Product>();

                // Dodatkowa możliwa logika
                // do DOOKREŚLENIA (!) specyficznych pól, takich
                // jak ranga produktu
                // i specyficzne informacje o produkcie

                unitOfWork.ProductRepository.Update(oProduct);

                unitOfWork.EventsRepository
                  .AddEventHistory($"Aktualizacja dostawcy {inputData.ProductName} o identyfikatorze {inputData.ProductID}");

                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
        }
        #endregion
    }

    public IEnumerable<ItemDTO> GetProductsCollection()
    {
        List<ItemDTO> retProducts = new List<ItemDTO>();

        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                IEnumerable<Product> allProducts = unitOfWork.ProductRepository.GetItemsList();
                #region Sekcja Mapowania
                foreach (var currentProduct in allProducts)
                {
                    retProducts.Add(new ItemDTO // Tutaj wymuszone ręczne mapowanie
                                                // z powodu niezgodności nazw
                    {
                        Id = currentProduct.ProductID,
                        Name = currentProduct.ProductName
                    });

                }
                #endregion

                unitOfWork.EventsRepository
                    .AddEventHistory($"Pobrano {retProducts.Count} dostawców");

                unitOfWork.Commit();

                return retProducts;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion



        return retProducts;
    }
    public IEnumerable<ItemProductGridDTO> GetProductsCollectionUsing(int idValue)
    {
        List<ItemProductGridDTO> retProducts = new List<ItemProductGridDTO>();

        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                IEnumerable<Product> allProducts = unitOfWork.ProductRepository.GetItemsList();


                #region Sekcja Mapowania
                foreach (var oProduct in allProducts)
                {
                    var productDto = oProduct.Adapt<ItemProductGridDTO>();

                    if (productDto.SupplierID == idValue)  // Logika filtrowania według ID dostawcy
                        retProducts.Add(productDto);      // Tutaj trafiają już przefiltrowane dane 
                }
                #endregion

                unitOfWork.EventsRepository
          .AddEventHistory($"Pobrano {retProducts.Count} dostawców");

                unitOfWork.Commit();

                return retProducts;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion



        return retProducts;
    }

    IEnumerable<ItemDTO> IProductService.GetSuppliersCollection()
    {
        List<ItemDTO> retSuppliers = new List<ItemDTO>();

        try
        {

            // Wywołanie tranzytowe wewnątrz jednej klasy serwisowej
            IEnumerable<ItemSupplierGridDTO> itemSupplierGridDtos = GetSuppliersCollection();

            // Mapowanie otrzymanych danych
            foreach (ItemSupplierGridDTO itemSupplierGridDto in itemSupplierGridDtos)
            {
                retSuppliers.Add(new ItemDTO
                {
                    Id = itemSupplierGridDto.SupplierId,
                    Name = itemSupplierGridDto.CompanyName
                });
            }

            return retSuppliers;

        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion

        return retSuppliers;
    }

    #endregion

    #region ORDER SPECIFIC
    public IEnumerable<ItemOrderGridDTO> GetOrdersCollectionUsing(int idValue)
    {
        List<ItemOrderGridDTO> retProducts = new List<ItemOrderGridDTO>();

        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                IEnumerable<Order> allOrders = unitOfWork.OrderRepository.GetItemsList();


                #region Sekcja Mapowania
                foreach (var currentOrder in allOrders)
                {
                    ItemOrderGridDTO orderDto = currentOrder.Adapt<ItemOrderGridDTO>();

                    if (orderDto.CustomerId == idValue)        // Logika filtrowania według ID klienta
                        retProducts.Add(orderDto);             // Tutaj trafiają już przefiltrowane dane
                }
                #endregion

                unitOfWork.EventsRepository
                    .AddEventHistory($"Pobrano {retProducts.Count} dostawców");

                unitOfWork.Commit();

                return retProducts;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion



        return retProducts;
    }

    public IEnumerable<ItemOrderDetailsGridDTO> GetOrdersTabCollectionUsing(int idValue)
    {
        List<ItemOrderDetailsGridDTO> retOrderTabCollection = new List<ItemOrderDetailsGridDTO>();

        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                IEnumerable<OrderDetails> allOrdersTabs = unitOfWork.OrderDetailsRepository
                    .GetItemsList();


                #region Sekcja Mapowania
                foreach (var currentTab in allOrdersTabs)
                {
                    var orderTabDto = currentTab.Adapt<ItemOrderDetailsGridDTO>();

                    if (currentTab.OrderId == idValue)             // Logika filtrowania według ID zamówienia
                        retOrderTabCollection.Add(orderTabDto);    // Tutaj trafiają już przefiltrowane dane
                }
                #endregion

                unitOfWork.EventsRepository
                    .AddEventHistory($"Pobrano {retOrderTabCollection.Count} dostawców");

                unitOfWork.Commit();

            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion



        return retOrderTabCollection;
    }

    public void DeleteOrderUsingMapped(int idValue)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                unitOfWork.OrderRepository.Delete(idValue);

                unitOfWork.EventsRepository
                    .AddEventHistory($"Usunięcie produkta o identyfikatorze {idValue}");

                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
        }
        #endregion
    }

    public void DeleteOrderTabItemUsing(int idValue)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                unitOfWork.OrderDetailsRepository.Delete(idValue);

                unitOfWork.EventsRepository
                    .AddEventHistory($"Usunięcie produkta o identyfikatorze {idValue}");

                unitOfWork.Commit();
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
        }
        #endregion
    }

    /// <summary>
    /// Główna metoda wpływająca na PONOWNE OKREŚLENIE
    /// buforowanego obiektu-agregatu
    /// zgodnie z wybraną opcją zamówienia
    /// </summary>
    /// <param name="idValue">Identyfikator w tabeli Orders</param>
    /// <returns></returns>
    public bool DocumentRedefinitionUsing(int idValue)
    {
        bool retResult = false;

        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                IEnumerable<Order> allOrders = unitOfWork.OrderRepository
                    .GetItemsList();

                bool isOrderFound = false;

                #region Sekcja mapowania danych na nagłówek dokumentu
                foreach (var oCurrentOrder in allOrders)
                {
                    if (oCurrentOrder.OrderId == idValue)
                    {
                        this.DocumentModel.HeaderDocument = oCurrentOrder;
                        isOrderFound = true;
                        break;
                    }
                }
                #endregion

                #region Sekcja mapowania danych na obszar tabelaryczny dokumentu
                if (isOrderFound)
                {
                    IEnumerable<OrderDetails> orderDetails = unitOfWork.OrderDetailsRepository
                        .GetItemsList();

                    this.DocumentModel.OrderDetails.Clear();   // Obowiązkowe wstępne czyszczenie

                    foreach (OrderDetails oCurrentTab in orderDetails)
                    {
                        if (oCurrentTab.OrderId == this.DocumentModel
                                .HeaderDocument.OrderId)
                            this.DocumentModel.OrderDetails.Add(oCurrentTab);
                    }
                }
                else
                    throw new Exception($"No data for {idValue}");
                #endregion


                // Ponowne określenie następuje dokładnie tutaj. Ponowne przeliczenie specyficznych, modelowych parametrów, dotyczących tylko zamówienia: SummaryValue, OrderSpecificData,..., (itp)




                unitOfWork.EventsRepository
                    .AddEventHistory($"Aktywowano zamówienie # {this.DocumentModel.HeaderDocument.OrderNumber}");

                unitOfWork.Commit();
                retResult = true;

                return retResult;
            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion


        return retResult;
    }

    /// <summary>
    /// Główna metoda zapewniająca zapis kolejnego
    /// elementu, należącego do tabelarycznej części obiektu-dokumentu zamówienia
    /// </summary>
    /// <param name="itemTab"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void SetTabItemUsing(ItemOrderDetailsGridDTO itemTab)
    {
        try
        {
            using (var unitOfWork = new UnitOfWork(connectionString))
            {
                OrderDetails orderTabDetails = itemTab.Adapt<OrderDetails>();

                // Dodatkowa możliwa logika
                // dla Dookreślenia (!) specyficznych pól. Jeśli potrzebne


                unitOfWork.OrderDetailsRepository.Create(orderTabDetails);

                unitOfWork.EventsRepository
                    .AddEventHistory($"Stworzono nowy element części tabelarycznej {orderTabDetails.ProductName} o identyfikatorze {orderTabDetails.ProductID}");

                unitOfWork.Commit();

            }
        }

        #region Blok Catch
        catch (Exception ex)
        {
            // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
            // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
        }
        #endregion
    }

    IEnumerable<ItemDTO> IOrderService.GetCustomersCollection()
    {
        {
            List<ItemDTO> retCustomers = new List<ItemDTO>();

            try
            {

                // Przejściowe wywołanie w ramach jednej klasy serwisowej
                // pobieramy kolekcję obiektów klientów
                IEnumerable<ItemCustomerGridDTO> customersCollection = GetCustomersCollection();

                // Mapowanie otrzymanych danych
                foreach (ItemCustomerGridDTO oCustomerGridDto in customersCollection)
                {
                    retCustomers.Add(new ItemDTO
                    {
                        Id = oCustomerGridDto.CustomerId,
                        Name = oCustomerGridDto.Name
                    });
                }

                return retCustomers;

            }

            #region Blok Catch

            catch (Exception ex)
            {
                // W tym miejscu można dodać logowanie błędu, jeśli jest to wymagane
                // Na przykład: Logger.LogError(ex, "Błąd podczas dodawania dostawcy");
            }

            #endregion



            return retCustomers;
        }

    }
    #endregion

    #region GENERAL
    public void GeneralServiceAction()
    {
        throw new NotImplementedException();
    }
    #endregion
}