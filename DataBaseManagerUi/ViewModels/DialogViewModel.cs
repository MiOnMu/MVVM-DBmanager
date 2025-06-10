using System.Globalization;
using DataBaseManager.AppService.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DataBaseManagerUi.ViewModels;

public class DialogViewModel : ObservableObject
{

    #region Fields
    private readonly IDbAppService _appService;
    #endregion

    #region Ctors
    public DialogViewModel(IDbAppService appService)
    { _appService = appService; }
    #endregion

}