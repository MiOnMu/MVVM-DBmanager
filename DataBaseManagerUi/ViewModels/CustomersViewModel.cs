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

namespace DataBaseManagerUi.ViewModels;

public class CustomersViewModel : ObservableObject
{
    #region Fields
    private readonly ICustomerService _appService;
    private readonly IDialogService _dialogService;
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
        IDialogService dialogService)
    {
        _appService = appService;
        _dialogService = dialogService;

        // Pobieramy kolekcję z serwisu
        var customersCollection = _appService.GetCustomersCollection();

        // Konwertujemy na ObservableCollection dla WPF
        Customers = new ObservableCollection<ItemCustomerGridDTO>(customersCollection);


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

        ItemCustomerGridDTO oNewDto = new ItemCustomerGridDTO();
        this.Adapt(oNewDto);           // Mapowanie danych
        if (IsValidNewDtoOf(oNewDto))  // Walidacja danych wejściowych
        {
            bool resulAddCustomerAction = _appService.AddCustomer(oNewDto);   // Ta gałąź zostanie wykonana, jeśli walidacja zakończy się pomyślnie
            if (!resulAddCustomerAction)
                _dialogService.ShowMessageBox(this,                   // A ta gałąź - jeśli wystąpił błąd podczas dodawania
                    $"$Błąd podczas próby dodania {oNewDto.Name}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            _dialogService.ShowMessageBox(this,                       // A ta gałąź - jeśli dane będą nieprawidłowe
                "Nieprawidłowe dane wejściowe", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    /// <summary>
    /// Logika edycji
    /// </summary>
    private async void OnEditCustomerAsync()
    {
        if (SelectedCustomer != null)
        {
            ItemCustomerGridDTO editDto = new ItemCustomerGridDTO();
            this.Adapt(editDto);               // Mapowanie danych
            editDto.Id = SelectedCustomer.Id; // Koniecznie ustawiamy identyfikator !!

            if (IsValidNewDtoOf(editDto))                                 // Walidacja danych wejściowych
            {
                _appService.UpdateExistedCustomer(editDto);               // Ta gałąź zostanie wykonana, jeśli walidacja zakończy się pomyślnie
            }
            else
            {
                _dialogService.ShowMessageBox(this,                   // A ta gałąź - jeśli dane będą nieprawidłowe
                    "Nieprawidłowe dane wejściowe", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            _dialogService.ShowMessageBox(this,                   // Wyświetli się, jeśli użytkownik niczego nie wybrał z tabeli
                "Najpierw należy wybrać wiersz do edycji!", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
        }


    }

    /// <summary>
    /// Logika usuwania
    /// </summary>
    /// <param name="customer"></param>
    private async void OnDeleteCustomerAsync(ItemCustomerGridDTO customer)
    {
        _appService.DeleteCustomerUsing(customer.Id);
    }
    #endregion

    #region Methods

    private bool IsValidNewDtoOf(ItemCustomerGridDTO oDto)
    {
        return true; // TODO : Uzupełnić później rzeczywistą logiką walidacji danych, na razie zostaje tak
    }
    #endregion


}