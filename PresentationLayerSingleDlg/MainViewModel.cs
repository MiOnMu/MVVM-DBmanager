using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

namespace PresentationLayerSingleDlg;

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
        IServiceProvider serviceProvider)  // Wstrzykiwanie zależności IServiceProvider przez konstruktor
    {
        _dialogService = dialogService;
        _serviceProvider = serviceProvider;
    }
    #endregion


    #region Handlers

    /// <summary>
    /// Główna metoda obsługi do wyświetlania okna pomocniczego
    /// </summary>
    /// <returns></returns>
    private async Task OpenDialogAsync()
    {
        // Pobieranie obiektu DialogViewModel z kontenera wstrzykiwania zależności
        // korzystamy z service providera
        var dialogVM = _serviceProvider.GetRequiredService<DialogViewModel>();

        _dialogService.Show(this, dialogVM); // Polecenie do wyświetlenia okna
        // W tym miejscu wkracza do działania biblioteka MvvmDialogs
    }
    #endregion
}
