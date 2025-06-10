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
    public IRelayCommand ShowDialogCommand => new AsyncRelayCommand(OpenDialogAsync);
    #endregion


    #region Ctors

    /// <summary>
    /// Konstruktor z parametrami
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="serviceProvider"></param>
    public MainViewModel(
        IDialogService dialogService,    // Wstrzykiwanie zależności IDialogService przez konstruktor
        IServiceProvider serviceProvider) // Wstrzykiwanie zależności IServiceProvider przez konstruktor
    {
        _dialogService = dialogService;
        _serviceProvider = serviceProvider;
    }
    #endregion


    #region Handlers

    /// <summary>
    /// Główny handler do wyświetlania okna pomocniczego
    /// </summary>
    /// <returns></returns>
    private async Task OpenDialogAsync()
    {

        // Pobieranie obiektu DialogViewModel z kontenera wstrzykiwania zależności
        // działamy poprzez dostawcę usług (service provider)
        var dialogVM = _serviceProvider.GetRequiredService<DialogViewModel>();


        _dialogService.Show(this, dialogVM); // Właściwe polecenie wyświetlenia
        // W tym miejscu do pracy włącza się biblioteka MvvmDialogs

    }
    #endregion
}