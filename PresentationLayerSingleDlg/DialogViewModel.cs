using System.Globalization;
using AppServiceLayerSingleDlg;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PresentationLayerSingleDlg;

public class DialogViewModel : ObservableObject
{

    #region Fields
    private readonly IAppService _appService;
    #endregion

    #region Ctors
    public DialogViewModel(IAppService appService)
    {
        _appService = appService;
    }
    #endregion

}