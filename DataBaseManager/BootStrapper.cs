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

        services.AddLogging();

        // Rejestracja serwisów
        services.AddSingleton<IDialogService,   DialogService>();

        services.AddSingleton<IDbAppService,    DbAppService>();

        services.AddSingleton<ICustomerService, DbAppService>();
        services.AddSingleton<ISupplierService, DbAppService>();
        services.AddSingleton<IProductService,  DbAppService>();
        services.AddSingleton<IOrderService,    DbAppService>();

        // Rejestracja ViewModeli
        services.AddTransient<MainViewModel>();
        services.AddTransient<CustomersViewModel>();
        services.AddTransient<SuppliersViewModel>();
        services.AddTransient<ProductsViewModel>();
        services.AddTransient<OrdersViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public void Run()
    {
        var mainWindow = new MainView  // Tworzymy główne okno i ustawiamy DataContext
        { DataContext = _serviceProvider.GetRequiredService<MainViewModel>() };
        mainWindow.Show();
    }
}