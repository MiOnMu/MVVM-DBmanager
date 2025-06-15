using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataBaseManager.AppService.Contracts;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;
using Mapster;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Logging;

namespace DataBaseManagerUi.ViewModels;

public class CustomersViewModel : ObservableObject
{
    #region Fields
    private readonly ICustomerService _appService;
    private readonly IDialogService _dialogService;
    private readonly ILogger<CustomersViewModel> _logger;
    #endregion

    #region Commands
    public ICommand EditCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand AddCustomerCommand { get; set; }
    #endregion

    #region Properties

    #region Customer Collection
    private ItemCustomerGridDTO _selectedCustomer;
    public ItemCustomerGridDTO SelectedCustomer   // Ta właściwość zmienia się po kliknięciu myszą na pozycji w tabeli
    {
        get => _selectedCustomer;
        set
        {
            if (SetProperty(ref _selectedCustomer, value))
            {
                Name = _selectedCustomer?.Name;
                ContactName = _selectedCustomer?.ContactName;
                Address = _selectedCustomer?.Address;
                City = _selectedCustomer?.City;
                Country = _selectedCustomer?.Country;
                Email = _selectedCustomer?.Email;
                Phone = _selectedCustomer?.Phone;
                PostalCode = _selectedCustomer?.PostalCode;
                Region = _selectedCustomer?.Region;
            }
        }
    }

    private ObservableCollection<ItemCustomerGridDTO> _customers;
    public ObservableCollection<ItemCustomerGridDTO> Customers
    {
        get => _customers;
        set => SetProperty(ref _customers, value);
    }
    #endregion

    #region Contact Name
    private string _contactName;
    public string ContactName
    {
        get => _contactName;
        set => SetProperty(ref _contactName, value);
    }
    #endregion

    #region  Name
    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    #endregion

    #region Phone
    private string _phone;
    public string Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }
    #endregion

    #region Email
    private string _email;
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }
    #endregion

    #region Address
    private string _address;
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }
    #endregion

    #region City
    private string _city;
    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }
    #endregion

    #region Region
    private string _region;
    public string Region
    {
        get => _region;
        set => SetProperty(ref _region, value);
    }
    #endregion

    #region PostalCode
    private string _postalCode;
    public string PostalCode
    {
        get => _postalCode;
        set => SetProperty(ref _postalCode, value);
    }
    #endregion

    #region Country
    private string _country;
    public string Country
    {
        get => _country;
        set => SetProperty(ref _country, value);
    }
    #endregion

    #endregion

    #region Ctors
    public CustomersViewModel(
        IDbAppService appService,
        IDialogService dialogService,
        ILogger<CustomersViewModel> logger)
    {
        _appService = appService;
        _dialogService = dialogService;

        _logger.LogInformation("Initialization of CustomersViewModel");

        ReLoadingGridData();


        // Inicjalizacja poleceń
        EditCommand = new RelayCommand(OnEditCustomerAsync);                   // Polecenie do edycji klienta, przyjmuje ItemCustomerGridDTO jako parametr
        DeleteCommand = new RelayCommand<ItemCustomerGridDTO>(OnDeleteCustomerAsync); // Polecenie do usunięcia klienta, przyjmuje ItemCustomerGridDTO jako parametr
        AddCustomerCommand = new RelayCommand(OnConfirmationAddAsync);               // Polecenie do dodawania nowego klienta, nie przyjmuje parametrów
    }
    #endregion

    #region Handlers

    /// <summary>
    /// Logika zapisu do bazy danych
    /// </summary>
    private async void OnConfirmationAddAsync()
    {
        _logger.LogInformation("Attempting to add a new customer");

        ItemCustomerGridDTO oNewDto = new ItemCustomerGridDTO();
        this.Adapt(oNewDto);           // Mapowanie danych
        if (IsValidNewDtoOf(oNewDto))    // Walidacja danych wejściowych
        {
            bool addCustomerResult = _appService.AddCustomer(oNewDto);    // Ta gałąź zadziała, jeśli walidacja przejdzie pomyślnie
            if (!addCustomerResult)
            {
                _logger.LogError("Error while trying to add customer {Name}", oNewDto.Name);
                _dialogService.ShowMessageBox(this,                   // A ta gałąź - jeśli wystąpił błąd podczas dodawania
                            $"Error while trying to add {oNewDto.Name}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _logger.LogInformation("Customer {Name} was successfully added", oNewDto.Name);
                ReLoadingGridData();        // Jeśli w bazie danych dodawanie przebiegło pomyślnie -> Wymuszone odświeżenie tabeli

                _dialogService.ShowMessageBox(this,
          $"New customer {oNewDto.Name} has been successfully added", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            _logger.LogWarning("Validation failed when adding a customer");
            _dialogService.ShowMessageBox(this,                       // A ta gałąź - jeśli dane będą nieprawidłowe
                      "Invalid source data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Logika edycji
    /// </summary>
    private async void OnEditCustomerAsync()
    {
        if (SelectedCustomer != null)
        {
            _logger.LogInformation("Attempting to edit customer with Id={CustomerId}", SelectedCustomer.CustomerId);

            ItemCustomerGridDTO editDto = new ItemCustomerGridDTO();
            this.Adapt(editDto);           // Mapowanie danych
            editDto.CustomerId = SelectedCustomer.CustomerId; // Koniecznie ustawiamy identyfikator !!

            if (IsValidNewDtoOf(editDto))                                   // Walidacja danych wejściowych
            {
                _appService.UpdateExistedCustomer(editDto);       // Ta gałąź zadziała, jeśli walidacja przejdzie pomyślnie

                _logger.LogInformation("Customer {Name} (Id={CustomerId}) has been successfully updated", editDto.Name, editDto.CustomerId);

                // Ponowne odświeżenie zawartości tabeli
                ReLoadingGridData();

                _dialogService.ShowMessageBox(this,
                  $"Existing customer {editDto.Name} has been successfully updated", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _logger.LogWarning("Validation failed when editing customer with Id={CustomerId}", editDto.CustomerId);
                _dialogService.ShowMessageBox(this,       // A ta gałąź - jeśli dane będą nieprawidłowe
                            "Invalid source data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            _logger.LogWarning("Edit customer: no row selected for editing");
            _dialogService.ShowMessageBox(this,       // Wyświetli się, jeśli użytkownik nie wybrał niczego z tabeli
                      "You must first select a row to edit!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    /// <summary>
    /// Logika usuwania
    /// </summary>
    /// <param name="customer"></param>
    private async void OnDeleteCustomerAsync(ItemCustomerGridDTO customer)
    {
        if (customer == null)
        {
            _logger.LogWarning("Delete customer: null was passed");
            return;
        }

        _logger.LogInformation("Attempting to delete customer {Name} (Id={CustomerId})", customer.Name, customer.CustomerId);

        // Wywołanie procedury usuwania z bazy danych
        _appService.DeleteCustomerUsing(customer.CustomerId);

        _logger.LogInformation("Customer {Name} (Id={CustomerId}) was successfully deleted", customer.Name, customer.CustomerId);

        // Ponowne odświeżenie zawartości tabeli
        ReLoadingGridData();

        // Na koniec komunikat dialogowy o pomyślnym usunięciu
        _dialogService.ShowMessageBox(this,
      $"Deleted customer {customer.Name}", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Odświeżenie zawartości tabeli
    /// </summary>
    private void ReLoadingGridData()
    {
        _logger.LogInformation("Refreshing the contents of the customer table");

        // Pobieramy kolekcję z serwisu
        IEnumerable<ItemCustomerGridDTO> customersCollection = _appService.GetCustomersCollection();
        if (!customersCollection.Any())
        {
            _logger.LogError("Error loading customer data: collection is empty");
            _dialogService.ShowMessageBox(this,
              "Error loading customer data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Konwertujemy na ObservableCollection dla WPF
        Customers = new ObservableCollection<ItemCustomerGridDTO>(customersCollection);
    }

    private bool IsValidNewDtoOf(ItemCustomerGridDTO oDto)
    {
        // Można dodać logowanie przy rzeczywistej walidacji
        return true; // TODO : Uzupełnić później rzeczywistą logiką sprawdzania danych, na razie zostaje tak
    }
    #endregion
}