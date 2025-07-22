using CommunityToolkit.Mvvm.Input;
using DataBaseManager.AppService.Contracts;
using DataBaseManagerUi.Bases;
using Microsoft.Extensions.Logging;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.MessageBox;
using System;
using System.Windows;
using System.Windows.Input;


namespace DataBaseManagerUi.ViewModels;

public class LogonViewModel : BaseViewModel, IModalDialogViewModel
{
    #region Fields
    private readonly ILogger<LogonViewModel> _logger;
    private readonly ILogonService _appService;
    private readonly IDialogService _dialogService;
    #endregion

    #region Properties
    #region Is User Admin
    private bool _isUserAdmin;
    public bool IsUserAdmin => _isUserAdmin;

    #endregion

    #region Result Dialog
    public bool? _dialogResult = null;
    public bool? DialogResult => _dialogResult;
    #endregion

    #region New User Tab Visibility 
    private Visibility _newUserTabVisibility;
    public Visibility NewUserTabVisibility
    {
        get => _newUserTabVisibility;
        set
        {
            _newUserTabVisibility = value;
            OnPropertyChanged(nameof(NewUserTabVisibility));
        }
    }
    #endregion

    #region Is Enabled Close
    private bool _isEnabledClose;
    public bool IsEnabledClose
    {
        get => _isEnabledClose;
        set
        {
            _isEnabledClose = value;
            OnPropertyChanged(nameof(IsEnabledClose));
        }
    }
    #endregion

    #region Current User 
    private string _currentUser;
    public string CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    #endregion

    #region Password 
    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    #endregion

    #region New User 
    private string _newUser;
    public string NewUser
    {
        get => _newUser;
        set => SetProperty(ref _newUser, value);
    }
    #endregion

    #region Password New User
    private string _passwordNewUser;
    public string PasswordNewUser
    {
        get => _passwordNewUser;
        set => SetProperty(ref _passwordNewUser, value);
    }
    #endregion
    #endregion


    #region Commands
    public ICommand LogonCommand { get; set; }
    public ICommand CloseCommand { get; set; }
    public ICommand AddUserCommand { get; set; }
    #endregion

    #region Ctors
    public LogonViewModel(
        ILogonService appService,
        ILogger<LogonViewModel> logger,
        IDialogService dialogService)
    {
        _appService = appService;
        _dialogService = dialogService;
        _logger = logger;
        OnLoadCommand = new RelayCommand(OnPrimaryLoadingAsync);
        LogonCommand = new RelayCommand(OnLogonAsync);
        CloseCommand = new RelayCommand(OnCloseAsync);
        AddUserCommand = new RelayCommand(OnUserAddAsync);
    }
    #endregion

    #region Handlers

    private async void OnUserAddAsync()
    {
        try
        {
            if ((string.IsNullOrEmpty(PasswordNewUser)) || (string.IsNullOrEmpty(NewUser)))
                throw new ArgumentOutOfRangeException();

            if (_appService.AddNewUser(PasswordNewUser, NewUser))
            {
                _dialogService.ShowMessageBox(this, new MessageBoxSettings
                {
                    Caption = "Ok",
                    Icon = MessageBoxImage.Information,
                    Button = MessageBoxButton.OK,
                    MessageBoxText = $"User {NewUser} already in the database!"
                });

                // Aktywujemy mo¿liwoœæ zamkniêcia,
                // co oznacza przejœcie do g³ównego etapu pracy
                // tej aplikacji
                AppState = "VisualStateClose";
            }

            else
            {

                _dialogService.ShowMessageBox(this, new MessageBoxSettings
                {
                    Caption = "Error input data",
                    Icon = MessageBoxImage.Error,
                    Button = MessageBoxButton.OK,
                    MessageBoxText = "Error adding of the user"
                });
            }
        }
        #region Catch Blocks
        catch (ArgumentOutOfRangeException)
        {
            AppState = "VisualStateErrorPass";
            _dialogService.ShowMessageBox(this, new MessageBoxSettings
            {
                Caption = "Error input data",
                Icon = MessageBoxImage.Error,
                Button = MessageBoxButton.OK,
                MessageBoxText = "Error input data"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        #endregion

    }

    private async void OnPrimaryLoadingAsync()
    {
        // Pocz¹tkowe ukrycie tej zak³adki,
        // która zgodnie ze scenariuszem
        // bêdzie dostêpna tylko dla administratorów
        NewUserTabVisibility = Visibility.Hidden;

        // Domyœlnie na starcie ten przycisk jest zablokowany,
        // dopóki u¿ytkownik nie poda poprawnego has³a
        IsEnabledClose = false;
    }
    private async void OnLogonAsync()
    {
        _logger.LogInformation("Attempt to logon");
        _dialogResult = _appService.IsUserValid(this.Password, CurrentUser, out _isUserAdmin);

        if (_dialogResult == null || _dialogResult == false)
        {
            // Aktywujemy stan b³êdu
            AppState = "VisualStateError";

            _dialogService.ShowMessageBox(this,
                new MessageBoxSettings
                {
                    Button = MessageBoxButton.OK,
                    Caption = "Error...",
                    Icon = MessageBoxImage.Error,
                    MessageBoxText = "The Password or User Name is incorrect"
                });
            _logger.LogInformation("Logon failure");
        }
        else
        {
            _dialogService.ShowMessageBox(this,
                new MessageBoxSettings
                {
                    Button = MessageBoxButton.OK,
                    Caption = "Ok",
                    Icon = MessageBoxImage.Information,
                    MessageBoxText = "Password is correct"
                });
            // Aktywujemy stan poprawnoœci
            AppState = "VisualStateValid";

            // Podœwietlenie przycisku zamykania przez VisualState
            AppState = "VisualStateClose";

            if (_isUserAdmin)  // Sprawdzanie opcji administratora
            {
                // Wyœwietlamy panel dodawania u¿ytkowników
                // Tylko dostêpne dla administratorów 
                NewUserTabVisibility = Visibility.Visible;

            }

            // Ostateczne odblokowanie przycisku
            IsEnabledClose = true;

        }
    }
    private async void OnCloseAsync()
    {
        _logger.LogInformation("Closing Logon View");
        base.CloseAssociatedView();

    }
    #endregion
}