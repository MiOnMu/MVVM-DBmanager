using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmDialogs;
using System.Configuration;
using System.Data;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using DataBaseManager;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;

namespace DataBaseManager
{

    public partial class App : Application
    {
        /// <summary>
        /// Ta metoda zostanie wykonana jako pierwsza w aplikacji
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new BootStrapper();
            bootstrapper.Run();
        }
    }

}