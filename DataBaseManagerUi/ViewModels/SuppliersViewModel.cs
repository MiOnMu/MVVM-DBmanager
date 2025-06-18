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

public class SuppliersViewModel : ObservableObject
{
    #region Fields
    private readonly ISupplierService _appService;
    private readonly IDialogService _dialogService;
    private readonly ILogger<SuppliersViewModel> _logger;
    #endregion

    #region Commands
    public ICommand EditCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand AddSupplierCommand { get; set; }
    #endregion

    #region Properties

    #region Supplier Collection
    private ItemSupplierGridDTO _selectedSupplier;
    public ItemSupplierGridDTO SelectedSupplier   // Ta właściwość zmienia się po kliknięciu myszą na pozycji w tabeli
    {
        get => _selectedSupplier;
        set
        {
            if (SetProperty(ref _selectedSupplier, value))
            {
                CompanyName = _selectedSupplier?.CompanyName;
                ContactName = _selectedSupplier?.ContactName;
                Email = _selectedSupplier?.Email;
                Phone = _selectedSupplier?.Phone;
            }
        }
    }

    private ObservableCollection<ItemSupplierGridDTO> _suppliers;
    public ObservableCollection<ItemSupplierGridDTO> Suppliers
    {
        get => _suppliers;
        set => SetProperty(ref _suppliers, value);
    }
    #endregion

    #region Company Name
    private string _companyName;
    public string CompanyName
    {
        get => _companyName;
        set => SetProperty(ref _companyName, value);
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

    #endregion

    #region Ctors
    public SuppliersViewModel(
        IDbAppService appService,
        IDialogService dialogService,
        ILogger<SuppliersViewModel> logger)
    {
        _appService = appService;
        _dialogService = dialogService;
        _logger = logger;

        _logger.LogInformation("Initialization of SuppliersViewModel");

        ReLoadingGridData();


        // Inicjalizacja poleceń
        EditCommand = new RelayCommand(OnEditSupplierAsync);                   // Polecenie do edycji dostawcy, przyjmuje ItemSupplierGridDTO jako parametr
        DeleteCommand = new RelayCommand<ItemSupplierGridDTO>(OnDeleteSupplierAsync); // Polecenie do usunięcia dostawcy, przyjmuje ItemSupplierGridDTO jako parametr
        AddSupplierCommand = new RelayCommand(OnConfirmationAddAsync);               // Polecenie do dodawania nowego dostawcy, nie przyjmuje parametrów
    }
    #endregion

    #region Handlers

    /// <summary>
    /// Logika zapisu do bazy danych
    /// </summary>
    private async void OnConfirmationAddAsync()
    {
        _logger.LogInformation("Attempting to add a new supplier");

        ItemSupplierGridDTO oNewDto = new ItemSupplierGridDTO();
        this.Adapt(oNewDto);           // Mapowanie danych
        if (IsValidNewDtoOf(oNewDto))    // Walidacja danych wejściowych
        {
            bool resulAddSupplierAction = _appService.AddSupplier(oNewDto);    // Ta gałąź zadziała, jeśli walidacja przejdzie pomyślnie
            if (!resulAddSupplierAction)
            {
                _logger.LogError("Error while trying to add supplier {CompanyName}", oNewDto.CompanyName);
                _dialogService.ShowMessageBox(this,                   // A ta gałąź - jeśli wystąpił błąd podczas dodawania
                            $"Error while trying to add {oNewDto.CompanyName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _logger.LogInformation("Supplier {CompanyName} was successfully added", oNewDto.CompanyName);
                ReLoadingGridData();        // Jeśli w bazie danych dodawanie przebiegło pomyślnie -> Wymuszone odświeżenie tabeli

                _dialogService.ShowMessageBox(this,
          $"New supplier {oNewDto.CompanyName} has been successfully added", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            _logger.LogWarning("Validation failed when adding a supplier");
            _dialogService.ShowMessageBox(this,                       // A ta gałąź - jeśli dane będą nieprawidłowe
                      "Invalid source data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Logika edycji
    /// </summary>
    private async void OnEditSupplierAsync()
    {
        if (SelectedSupplier != null)
        {
            _logger.LogInformation("Attempting to edit supplier with Id={SupplierId}", SelectedSupplier.SupplierId);

            ItemSupplierGridDTO editDto = new ItemSupplierGridDTO();
            this.Adapt(editDto);           // Mapowanie danych
            editDto.SupplierId = SelectedSupplier.SupplierId; // Koniecznie ustawiamy identyfikator !!

            if (IsValidNewDtoOf(editDto))                                   // Walidacja danych wejściowych
            {
                _appService.UpdateExistedSupplier(editDto);       // Ta gałąź zadziała, jeśli walidacja przejdzie pomyślnie

                _logger.LogInformation("Supplier {CompanyName} (Id={SupplierId}) has been successfully updated", editDto.CompanyName, editDto.SupplierId);

                // Ponowne odświeżenie zawartości tabeli
                ReLoadingGridData();

                _dialogService.ShowMessageBox(this,
                  $"Existing supplier {editDto.CompanyName} has been successfully updated", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _logger.LogWarning("Validation failed when editing supplier with Id={SupplierId}", editDto.SupplierId);
                _dialogService.ShowMessageBox(this,       // A ta gałąź - jeśli dane będą nieprawidłowe
                            "Invalid source data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            _logger.LogWarning("Edit supplier: no row selected for editing");
            _dialogService.ShowMessageBox(this,       // Wyświetli się, jeśli użytkownik nie wybrał niczego z tabeli
                      "You must first select a row to edit!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    /// <summary>
    /// Logika usuwania
    /// </summary>
    /// <param name="supplier"></param>
    private async void OnDeleteSupplierAsync(ItemSupplierGridDTO supplier)
    {
        if (supplier == null)
        {
            _logger.LogWarning("Delete supplier: null was passed");
            return;
        }

        _logger.LogInformation("Attempting to delete supplier {CompanyName} (Id={SupplierId})", supplier.CompanyName, supplier.SupplierId);

        // Wywołanie procedury usuwania z bazy danych
        _appService.DeleteSupplierUsing(supplier.SupplierId);

        _logger.LogInformation("Supplier {CompanyName} (Id={SupplierId}) was successfully deleted", supplier.CompanyName, supplier.SupplierId);

        // Ponowne odświeżenie zawartości tabeli
        ReLoadingGridData();

        // Na koniec komunikat dialogowy o pomyślnym usunięciu
        _dialogService.ShowMessageBox(this,
      $"Deleted supplier {supplier.CompanyName}", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Odświeżenie zawartości tabeli
    /// </summary>
    private void ReLoadingGridData()
    {
        _logger.LogInformation("Refreshing the contents of the supplier table");

        // Pobieramy kolekcję z serwisu
        IEnumerable<ItemSupplierGridDTO> suppliersCollection = _appService.GetSuppliersCollection();
        if (!suppliersCollection.Any())
        {
            _logger.LogError("Error loading supplier data: collection is empty");
            _dialogService.ShowMessageBox(this,
              "Error loading supplier data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Konwertujemy na ObservableCollection dla WPF
        Suppliers = new ObservableCollection<ItemSupplierGridDTO>(suppliersCollection);
    }

    private bool IsValidNewDtoOf(ItemSupplierGridDTO oDto)
    {
        // Można dodać logowanie przy rzeczywistej walidacji
        return true; // TODO : Uzupełnić później rzeczywistą logiką sprawdzania danych, na razie zostaje tak
    }
    #endregion
}