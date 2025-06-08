using CommunityToolkit.Mvvm.DependencyInjection;
using MvvmDialogs;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace protoSingleDlg
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
