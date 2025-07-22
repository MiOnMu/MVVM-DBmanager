using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmDialogs;
using System.Configuration;
using System.Data;
using System.Windows;
using DataBaseManager;
using Microsoft.Extensions.DependencyInjection;

namespace DataBaseManager
{
    /// <summary>
    /// Ta metoda zostanie wykonana jako pierwsza w aplikacji
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new BootStrapper();
            bootstrapper.Run();
        }
    }

}