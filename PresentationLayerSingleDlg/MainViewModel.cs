using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

namespace PresentationLayerSingleDlg;

public class MainViewModel : ObservableObject
{

    #region Fields
    private readonly IDialogService   _dialogService;
    private readonly IServiceProvider _serviceProvider;
    #endregion

    #region Commands
    public IRelayCommand ShowDialogCommand => new AsyncRelayCommand(OpenDialogAsync);
    #endregion


    #region Ctors

    /// <summary>
    /// Конструктор с парамтерами
    /// </summary>
    /// <param name="dialogService"></param>
    /// <param name="serviceProvider"></param>
    public MainViewModel(
        IDialogService   dialogService,    // Внедрение зависимости IDialogService через конструктор
        IServiceProvider serviceProvider)  // Внедрение зависимости IServiceProvider через конструктор
    {
        _dialogService   = dialogService;
        _serviceProvider = serviceProvider;
    }
    #endregion


    #region Handlers

    /// <summary>
    /// Основной обработчик для отображения вспомогательного окна
    /// </summary>
    /// <returns></returns>
    private async Task OpenDialogAsync()
    {

        // Извлечение обекта DialogViewModel из контейнера внедрения зависимостей
        // действуем через сервис- провайдер
        var dialogVM = _serviceProvider.GetRequiredService<DialogViewModel>();


        _dialogService.Show(this,dialogVM); // Собственно сама команда на отображение
        // Тут как раз и включается в работу библиотека MvvmDialogs

    }
    #endregion
}