using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataBaseManager.AppService.Contracts;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;
using Mapster;
using Microsoft.Extensions.Logging;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace DataBaseManagerUi.ViewModels;

public class OrdersViewModel : ObservableObject
{
    #region Fields
    private readonly IOrderService _appService;
    private readonly IDialogService _dialogService;
    private readonly ILogger<OrdersViewModel> _logger;
    #endregion

    #region Commands
    public ICommand EditCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand DeleteTabItemCommand { get; set; }
    public ICommand AddOrderCommand { get; set; }
    #endregion

    #region Properties
    #region Orders
    #region Grid Orders Collection
    private ObservableCollection<ItemOrderGridDTO> _itemsGrid = new ObservableCollection<ItemOrderGridDTO>();
    public ObservableCollection<ItemOrderGridDTO> ItemsGrid
    {
        get => _itemsGrid;
        set
        {
            if (_itemsGrid != value)
            {
                _itemsGrid = value;
                OnPropertyChanged(nameof(ItemsGrid));
            }
        }
    }
    #endregion

    #region    Selected Order
    private ItemOrderGridDTO _selectedOrder;
    public ItemOrderGridDTO SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            if (SetProperty(ref _selectedOrder, value))
            {
                if (_appService.DocumentRedefinitionUsing(value.OrderId))
                {
                    IEnumerable<ItemOrderDetailsGridDTO> gridCollection = _appService
                        .GetOrdersTabCollectionUsing(value.OrderId);

                    if (gridCollection.Any())
                        ItemsGridDetails = new ObservableCollection<ItemOrderDetailsGridDTO>(gridCollection);
                    OrderNumber = SelectedOrder?.OrderNumber;
                    ShipCountry = SelectedOrder?.ShipCountry;
                    ShipPostalCode = SelectedOrder?.ShipPostalCode;
                    ShipRegion = SelectedOrder?.ShipRegion;
                    ShipCity = SelectedOrder?.ShipCity;
                    ShipAddress = SelectedOrder?.ShipAddress;
                    ShipName = SelectedOrder?.ShipName;
                    ShipVia = (int)SelectedOrder?.ShipVia;
                    ShippedDate = (DateTime)SelectedOrder?.ShippedDate;
                    RequiredDate = (DateTime)SelectedOrder?.RequiredDate;
                    OrderDate = (DateTime)SelectedOrder?.ShippedDate;
                }

            }
        }
    }

    #endregion
    #endregion

    #region Customers Combo
    #region Combo Box Collection
    private ObservableCollection<ItemDTO> _items = new ObservableCollection<ItemDTO>();
    public ObservableCollection<ItemDTO> Items
    {
        get => _items;
        set
        {
            if (_items != value)
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }
    }
    #endregion

    #region Selected Item 
    private ItemDTO _selectedItem;
    public ItemDTO SelectedItem
    {
        get { return _selectedItem; }
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                ReLoadingGridData(value.Id);
            }
        }
    }
    #endregion
    #endregion

    #region Order Number
    private string _orderNumber;
    public string OrderNumber
    {
        get => _orderNumber;
        set => SetProperty(ref _orderNumber, value);
    }
    #endregion

    #region Ship Country
    private string _shipCountry;
    public string ShipCountry
    {
        get => _shipCountry;
        set => SetProperty(ref _shipCountry, value);
    }
    #endregion

    #region Ship Postal Code
    private string _shipPostalCode;
    public string ShipPostalCode
    {
        get => _shipPostalCode;
        set => SetProperty(ref _shipPostalCode, value);
    }
    #endregion

    #region Ship Region
    private string _shipRegion;
    public string ShipRegion
    {
        get => _shipRegion;
        set => SetProperty(ref _shipRegion, value);
    }
    #endregion

    #region Ship City
    private string _shipCity;
    public string ShipCity
    {
        get => _shipCity;
        set => SetProperty(ref _shipCity, value);
    }
    #endregion

    #region Ship Address
    private string _shipAddress;
    public string ShipAddress
    {
        get => _shipAddress;
        set => SetProperty(ref _shipAddress, value);
    }
    #endregion

    #region Ship Name
    private string _shipName;
    public string ShipName
    {
        get => _shipName;
        set => SetProperty(ref _shipName, value);
    }
    #endregion

    #region Ship Via
    private int _shipVia;
    public int ShipVia
    {
        get => _shipVia;
        set => SetProperty(ref _shipVia, value);
    }
    #endregion

    #region Shipped Date
    private DateTime _shippedDate;
    public DateTime ShippedDate
    {
        get => _shippedDate;
        set => SetProperty(ref _shippedDate, value);
    }
    #endregion

    #region Required Date
    private DateTime _requiredDate;
    public DateTime RequiredDate
    {
        get => _requiredDate;
        set => SetProperty(ref _requiredDate, value);
    }
    #endregion

    #region Order Date
    private DateTime _orderDate;
    public DateTime OrderDate
    {
        get => _orderDate;
        set => SetProperty(ref _orderDate, value);
    }
    #endregion

    //------------

    #region Orders Details
    #region Grid Order Details Collection
    private ObservableCollection<ItemOrderDetailsGridDTO> _itemsGridDetails = new ObservableCollection<ItemOrderDetailsGridDTO>();
    public ObservableCollection<ItemOrderDetailsGridDTO> ItemsGridDetails
    {
        get => _itemsGridDetails;
        set
        {
            if (_itemsGridDetails != value)
            {
                _itemsGridDetails = value;
                OnPropertyChanged(nameof(ItemsGridDetails));
            }
        }
    }
    #endregion

    #region    Selected Order Detail
    private ItemOrderDetailsGridDTO _selectedOrderDetail;
    public ItemOrderDetailsGridDTO SelectedOrderDetail
    {
        get => _selectedOrderDetail;
        set
        {
            if (SetProperty(ref _selectedOrderDetail, value))
            {
                if (_selectedOrderDetail != null)
                {
                    UnitPrice = SelectedOrderDetail?.UnitPrice;
                    Quantity = SelectedOrderDetail?.Quantity;
                    Discount = SelectedOrderDetail?.Discount;

                    foreach (ItemDTO itemsProduct in ItemsProducts)
                    {
                        if (itemsProduct.Id == SelectedOrderDetail.ProductID)
                        {
                            SelectedItemProduct = itemsProduct;
                            break;
                        }
                    }
                }
            }
        }
    }

    #endregion
    #endregion

    #region Products Combo
    #region Combo Box Collection
    private ObservableCollection<ItemDTO> _itemsProducts = new ObservableCollection<ItemDTO>();
    public ObservableCollection<ItemDTO> ItemsProducts
    {
        get => _itemsProducts;
        set
        {
            if (_itemsProducts != value)
            {
                _itemsProducts = value;
                OnPropertyChanged(nameof(ItemsProducts));
            }
        }
    }
    #endregion

    #region Selected Item 
    private ItemDTO _selectedItemProduct;
    public ItemDTO SelectedItemProduct
    {
        get => _selectedItemProduct;
        set
        {
            if (_selectedItemProduct != value)
            {
                _selectedItemProduct = value;
                OnPropertyChanged(nameof(SelectedItemProduct));
                // ReLoadingGridDetailData(value.Id);
            }
        }
    }
    #endregion
    #endregion

    #region Unit Price
    private decimal? _unitPrice;
    public decimal? UnitPrice
    {
        get => _unitPrice;
        set => SetProperty(ref _unitPrice, value);
    }
    #endregion

    #region Quantity
    private int? _quantity;
    public int? Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }
    #endregion

    #region Discount
    private decimal? _discount;
    public decimal? Discount
    {
        get => _discount;
        set => SetProperty(ref _discount, value);
    }
    #endregion


    #endregion

    #region Ctors
    public OrdersViewModel(
        IOrderService appService,
        IDialogService dialogService,
        ILogger<OrdersViewModel> logger)
    {
        _appService = appService;
        _dialogService = dialogService;
        _logger = logger;

        _logger.LogInformation("Initialization of OrdersViewModel");

        // Ładowanie danych klientów do comboboxa
        Items = new ObservableCollection<ItemDTO>(_appService.GetCustomersCollection());
        SelectedItem = Items[0];

        // Ładowanie danych produktów do comboboxa
        ItemsProducts = new ObservableCollection<ItemDTO>(_appService.GetProductsCollection());
        SelectedItemProduct = ItemsProducts[0];


        // Inicjalizacja poleceń
        EditCommand = new RelayCommand(OnEditOrderAsync);                  // Polecenie do edycji dostawcy, przyjmuje ItemProductGridDTO jako parametr
        DeleteCommand = new RelayCommand<ItemOrderGridDTO>(OnDeleteOrderAsync);      // Polecenie do usunięcia dostawcy, przyjmuje ItemProductGridDTO jako parametr
        DeleteTabItemCommand = new RelayCommand(OnDeleteTabItemAsync);
        AddOrderCommand = new RelayCommand(OnConfirmationAddAsync);                  // Polecenie do dodawania nowego dostawcy, nie przyjmuje parametrów
    }
    #endregion

    #region Handlers

    /// <summary>
    /// Logika zapisu do bazy danych
    /// </summary>
    private async void OnConfirmationAddAsync()
    {
        // Połączone mapowanie z różnych źródeł
        ItemOrderDetailsGridDTO oNewDto = new ItemOrderDetailsGridDTO
        {
            OrderId = SelectedOrderDetail.OrderId,
            ProductID = SelectedItemProduct.Id,
            ProductName = SelectedItemProduct.Name,
        };
        this.Adapt(oNewDto);

        _appService.SetTabItemUsing(oNewDto);
        ReLoadingGridDetailData(SelectedOrder.OrderId);


    }

    /// <summary>
    /// Logika edycji
    /// </summary>
    private async void OnDeleteTabItemAsync()
    {
        _appService.DeleteOrderTabItemUsing(SelectedOrderDetail.OrderDetailID);
        ReLoadingGridDetailData(SelectedOrder.OrderId);
    }

    /// <summary>
    /// Logika edycji
    /// </summary>
    private async void OnEditOrderAsync()
    {



    }

    /// <summary>
    /// Logika usuwania
    /// </summary>
    /// <param name="supplier"></param>
    private async void OnDeleteOrderAsync(ItemOrderGridDTO order)
    {
        if (order == null)
        {
            _logger.LogWarning("Delete order: null was passed");
            return;
        }
        _appService.DeleteOrderUsingMapped(order.OrderId);
        _logger.LogInformation($"Attempting to delete order Id={order.OrderId}");

        // Ponowne odświeżenie zawartości tabeli
        ReLoadingGridData(SelectedItem.Id);

        // Na koniec komunikat dialogowy o pomyślnym usunięciu
        _dialogService.ShowMessageBox(this,
            $"Deleted order {order.OrderId}",
            "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Ponowne zapełnienie zawartości tabeli, odnoszącej się do
    /// nagłówków zamówień
    /// </summary>
    private void ReLoadingGridData(int idValue)
    {
        _logger.LogInformation("Refreshing the contents of the product table");

        IEnumerable<ItemOrderGridDTO> ordersCollection = _appService.GetOrdersCollectionUsing(idValue);
        if (!ordersCollection.Any())
        {
            _logger.LogError("Error loading product data: collection is empty");
            _dialogService.ShowMessageBox(this,
                "Error loading product data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Konwertujemy na ObservableCollection dla WPF
        ItemsGrid = new ObservableCollection<ItemOrderGridDTO>(ordersCollection);
    }

    /// <summary>
    /// Ponowne zapełnienie zawartości tabeli, odnoszącej się do
    /// obszaru szczegółów
    /// </summary>
    /// <param name="idValue"></param>
    private void ReLoadingGridDetailData(int idValue)
    {
        _logger.LogInformation("Refreshing the contents of the detail product table");

        IEnumerable<ItemOrderDetailsGridDTO> ordersTabCollection = _appService.GetOrdersTabCollectionUsing(idValue);
        if (!ordersTabCollection.Any())
        {
            _logger.LogError("Error loading product data: collection is empty");
            _dialogService.ShowMessageBox(this,
                "Error loading tab items data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Konwertujemy na ObservableCollection dla WPF
        ItemsGridDetails = new ObservableCollection<ItemOrderDetailsGridDTO>(ordersTabCollection);

    }

    private bool IsValidNewDtoOf(ItemProductGridDTO oDto)
    {
        // Można dodać logowanie przy rzeczywistej walidacji
        return true; // TODO : Uzupełnić później rzeczywistą logiką sprawdzania danych, na razie zostaje tak
    }

    #endregion

}