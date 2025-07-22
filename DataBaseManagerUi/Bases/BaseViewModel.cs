using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace DataBaseManagerUi.Bases;

public class BaseViewModel : ObservableObject
{
    #region Commands
    public ICommand OnLoadCommand { get; set; }
    #endregion

    #region Common App State
    private string _appState;
    public string AppState
    {
        get => _appState;

        set
        {
            if (_appState != value)
            {
                _appState = value;
                OnPropertyChanged(nameof(AppState));
            }
        }
    }
    #endregion

    #region Methods
    public void CloseAssociatedView()
    {
        foreach (Window itemWindow in System.Windows.Application.Current.Windows)
        {
            if (itemWindow.DataContext == this)
                itemWindow.Close();
        }
    }
    #endregion

}