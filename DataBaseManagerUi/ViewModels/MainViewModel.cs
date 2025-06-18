using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

namespace DataBaseManagerUi.ViewModels;

public class MainViewModel : ObservableObject
{
    #region Fields
    private readonly IDialogService _dialogService;
    private readonly IServiceProvider _serviceProvider;
    #endregion

    #region Commands
    public IRelayCommand ShowCustomersCommand => new AsyncRelayCommand(OpenCustomersAsync);
    public IRelayCommand ShowSuppliersCommand => new AsyncRelayCommand(OpenSuppliersAsync);
    #endregion

    #region Ctors

    /// <summary>
    /// Konstruktor z parametrami
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="serviceProvider"></param>
    public MainViewModel(
        IDialogService dialogService,       // Wstrzykiwanie zależności IDialogService przez konstruktor
        IServiceProvider serviceProvider)   // Wstrzykiwanie zależności IServiceProvider przez konstruktor
    {
        _dialogService = dialogService;
        _serviceProvider = serviceProvider;
    }
    #endregion

    #region Handlers
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
    #endregion
}
