using DataBaseManager.AppService;
using DataBaseManager.AppService.Contracts;
using DataBaseManager.DataAccess;
using DataBaseManager.DataAccess.Contracts;
using DataBaseManagerUi.ViewModels;
using DataBaseManagerUi.Views;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

namespace DataBaseManager;

public class BootStrapper : IBootStrapper
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Specjalna klasa, której celem jest zapewnienie powiązania zależności wewnątrz siebie
    /// </summary>
    public BootStrapper()
    {
        var services = new ServiceCollection();

        // Rejestracja serwisów
        services.AddSingleton<IDialogService, DialogService>();
        services.AddTransient<IDbAppService, DbAppService>();
        services.AddTransient<ICustomerService, DbAppService>();

        // Rejestracja ViewModeli
        services.AddTransient<MainViewModel>();
        services.AddTransient<CustomersViewModel>();

        // Rejestracja repozytoriów
        services.AddSingleton<ICustomerRepository, CustomerRepository>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Run()
    {
        var mainWindow = new MainView  // Tworzymy główne okno i ustawiamy DataContext
        { DataContext = _serviceProvider.GetRequiredService<MainViewModel>() };
        mainWindow.Show();
    }
}