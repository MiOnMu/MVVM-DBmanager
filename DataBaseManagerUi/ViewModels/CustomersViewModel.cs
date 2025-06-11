using CommunityToolkit.Mvvm.ComponentModel;
using DataBaseManager.AppService.Contracts;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;
using System.Collections.ObjectModel;

namespace DataBaseManagerUi.ViewModels;

public class CustomersViewModel : ObservableObject
{
    #region Fields
    private readonly IDbAppService _appService;
    private ObservableCollection<ItemCustomerGridDTO> _customers;
    #endregion

    #region Properties
    public ObservableCollection<ItemCustomerGridDTO> Customers
    {
        get => _customers;
        set => SetProperty(ref _customers, value);
    }
    #endregion

    #region Ctors
    public CustomersViewModel(IDbAppService appService)
    {
        _appService = appService;

        // Pobieramy kolekcję z serwisu
        var customersCollection = _appService.GetCustomersCollection();

        // Konwertujemy na ObservableCollection dla WPF
        Customers = new ObservableCollection<ItemCustomerGridDTO>(customersCollection);
    }
    #endregion
}