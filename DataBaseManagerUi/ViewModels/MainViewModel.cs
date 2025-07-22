using MvvmDialogs;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using DataBaseManagerUi.Bases;

namespace DataBaseManagerUi.ViewModels;

public class MainViewModel : BaseViewModel
{
    #region Fields
    private readonly IServiceProvider _serviceProvider;
    private readonly IDialogService _dialogService;
    private readonly ILogger<MainViewModel> _logger;
    #endregion

    #region Properties


    #region Main View Enabling
    private bool _enableMainView;
    public bool EnableMainView
    {
        get => _enableMainView;

        set
        {
            _enableMainView = value;
            OnPropertyChanged(nameof(EnableMainView));
        }
    }
    #endregion

    #endregion

    #region Commands
    public IRelayCommand ShowCustomersCommand => new AsyncRelayCommand(OpenCustomersAsync);
    public IRelayCommand ShowSuppliersCommand => new AsyncRelayCommand(OpenSuppliersAsync);
    public IRelayCommand ShowProductsCommand => new AsyncRelayCommand(OpenProductsAsync);
    public IRelayCommand ShowOrdersCommand => new AsyncRelayCommand(OpenOrdersAsync);
    #endregion

    #region Ctors
    public MainViewModel(
        IDialogService dialogService,
        IServiceProvider serviceProvider,
        ILogger<MainViewModel> logger)
    {
        _dialogService = dialogService;
        _serviceProvider = serviceProvider;
        _logger = logger;

        _logger.LogInformation("Initialization of MainViewModel");

        OnLoadCommand = new RelayCommand(OnPrimaryLoadingAsync);
    }
    #endregion




    #region Handlers

    private async void OnPrimaryLoadingAsync()
    {

        // Aktywujemy stan
        AppState = "VisualStateStart";

        EnableMainView = true;
        var logonVM = _serviceProvider.GetRequiredService<LogonViewModel>();
        bool? dialogResult = _dialogService.ShowDialog(this, logonVM);

        if (dialogResult == true)
        {
            // Aktywujemy stan
            AppState = "VisualStateEnd";
        }
        else
        {
            _logger.LogInformation("Termination the application...");
            // Wyjście z aplikacji w przypadku niepowodzenia logowania
            System.Windows.Application.Current.Shutdown();
        }

    }
    /// <summary>
    /// Nowy handler do wyświetlania okna pomocniczego,
    /// </summary>
    /// <returns></returns>
    private async Task OpenCustomersAsync()
    {

        // Pobranie obiektu CustomersViewModel z kontenera wstrzykiwania zależności
        // działamy przez dostawcę usług (service provider)
        var customerVM = _serviceProvider.GetRequiredService<CustomersViewModel>();


        _dialogService.Show(this, customerVM); // Właściwe polecenie wyświetlenia
        // W tym miejscu do pracy wkracza biblioteka MvvmDialogs

    }

    private async Task OpenSuppliersAsync()
    {

        // Pobranie obiektu CustomersViewModel z kontenera wstrzykiwania zależności
        // działamy przez dostawcę usług (service provider)
        var supplierVM = _serviceProvider.GetRequiredService<SuppliersViewModel>();


        _dialogService.Show(this, supplierVM); // Właściwe polecenie wyświetlenia
        // W tym miejscu do pracy wkracza biblioteka MvvmDialogs

    }

    private async Task OpenProductsAsync()
    {

        var productsVM = _serviceProvider.GetRequiredService<ProductsViewModel>();


        _dialogService.Show(this, productsVM);


    }

    private async Task OpenOrdersAsync()
    {

        var ordersVM = _serviceProvider.GetRequiredService<OrdersViewModel>();


        _dialogService.Show(this, ordersVM);


    }
    #endregion
}