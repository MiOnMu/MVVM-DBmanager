using AppServiceLayerSingleDlg;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using PresentationLayerSingleDlg;


namespace protoSingleDlg;

public class BootStrapper : IBootStrapper
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Specjalna klasa, której celem jest powiązanie zależności wewnątrz siebie
    /// </summary>
    public BootStrapper()
    {
        var services = new ServiceCollection();

        // Rejestracja usług
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppService, AppService>();

        // Rejestracja ViewModeli
        services.AddTransient<MainViewModel>();
        services.AddTransient<DialogViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Run()
    {
        var mainWindow = new MainWindow   // Tworzenie głównego okna i ustawienie DataContext
        { DataContext = _serviceProvider.GetRequiredService<MainViewModel>() };
        mainWindow.Show();
    }
}