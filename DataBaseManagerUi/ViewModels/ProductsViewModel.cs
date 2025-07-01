using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataBaseManager.AppService.Contracts;
using DataBaseManager.CrossCutting.CommonDTOs;
using DataBaseManager.DataAccess.Contracts.DTOs;
using Mapster;
using Microsoft.Extensions.Logging;
using MvvmDialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace DataBaseManagerUi.ViewModels;

public class ProductsViewModel : ObservableObject
{
    #region Fields
    private readonly IProductService _appService;
    private readonly IDialogService _dialogService;
    private readonly ILogger<ProductsViewModel> _logger;
    #endregion

    #region Commands
    public ICommand EditCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public ICommand AddProductCommand { get; set; }
    #endregion

    #region Properties

    #region Products
    #region Grid Products Collection
    private ObservableCollection<ItemProductGridDTO> _itemsGrid = new ObservableCollection<ItemProductGridDTO>();
    public ObservableCollection<ItemProductGridDTO> ItemsGrid
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

    #region   Selected Product
    private ItemProductGridDTO _selectedProduct;
    public ItemProductGridDTO SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (SetProperty(ref _selectedProduct, value))
            {
                ProductName = _selectedProduct?.ProductName;
                UnitPrice = (decimal)_selectedProduct?.UnitPrice;
                Discontinued = (bool)_selectedProduct?.Discontinued;
            }
        }
    }

    #endregion
    #endregion

    #region Suppliers Combo
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

    #region Product Name
    private string _productName;
    public string ProductName
    {
        get => _productName;
        set => SetProperty(ref _productName, value);
    }
    #endregion

    #region Unit Price
    private decimal _unitPrice;
    public decimal UnitPrice
    {
        get => _unitPrice;
        set => SetProperty(ref _unitPrice, value);
    }
    #endregion

    #region Discontinued
    private bool _discontinued;
    public bool Discontinued
    {
        get => _discontinued;
        set => SetProperty(ref _discontinued, value);
    }
    #endregion

    #endregion

    #region Ctors
    public ProductsViewModel(
        IProductService appService,
        IDialogService dialogService,
        ILogger<ProductsViewModel> logger)
    {
        _appService = appService;
        _dialogService = dialogService;
        _logger = logger;

        _logger.LogInformation("Initialization of ProductsViewModel");

        foreach (ItemDTO itemDto in _appService.GetSuppliersCollection())
            Items.Add(itemDto);

        SelectedItem = Items[0];



        // Inicjalizacja poleceñ
        EditCommand = new RelayCommand(OnEditProductAsync);                   // Polecenie do edycji dostawcy, przyjmuje ItemProductGridDTO jako parametr
        DeleteCommand = new RelayCommand<ItemProductGridDTO>(OnDeleteProductAsync); // Polecenie do usuniêcia dostawcy, przyjmuje ItemProductGridDTO jako parametr
        AddProductCommand = new RelayCommand(OnConfirmationAddAsync);               // Polecenie do dodawania nowego dostawcy, nie przyjmuje parametrów
    }
    #endregion

    #region Handlers

    /// <summary>
    /// Logika zapisu do bazy danych
    /// </summary>
    private async void OnConfirmationAddAsync()
    {
        _logger.LogInformation("Attempting to add a new product");

        ItemProductGridDTO oNewDto = new ItemProductGridDTO();
        this.Adapt(oNewDto);                  // Mapowanie danych
        oNewDto.SupplierID = SelectedItem.Id; // Supplier ID
        if (IsValidNewDtoOf(oNewDto))         // Walidacja danych wejœciowych
        {
            bool resulAddProductAction = _appService.CreateNewProductUsingMapped(oNewDto);    // Ta ga³¹Ÿ zadzia³a, jeœli walidacja przejdzie pomyœlnie
            if (!resulAddProductAction)
            {
                _logger.LogError($"Error while trying to add product {oNewDto.ProductName}");
                _dialogService.ShowMessageBox(this,                   // A ta ga³¹Ÿ - jeœli wyst¹pi³ b³¹d podczas dodawania
                    $"Error while trying to add {oNewDto.ProductName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _logger.LogInformation($"Product {oNewDto.ProductName} was successfully added");
                ReLoadingGridData(SelectedItem.Id); // Supplier-ID

                _dialogService.ShowMessageBox(this,
                    $"New product {oNewDto.ProductName} has been successfully added", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        else
        {
            _logger.LogWarning("Validation failed when adding a customer");
            _dialogService.ShowMessageBox(this,                       // A ta ga³¹Ÿ - jeœli dane bêd¹ nieprawid³owe
                "Invalid source data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Logika edycji
    /// </summary>
    private async void OnEditProductAsync()
    {

        if (SelectedProduct != null)
        {
            string productName = SelectedProduct.ProductName;
            _logger.LogInformation($"Attempting to edit product with Id={SelectedProduct.ProductID}",
                SelectedProduct.ProductID);

            ItemProductGridDTO editDto = new ItemProductGridDTO();
            this.Adapt(editDto);           // Mapowanie danych
            editDto.SupplierID = SelectedProduct.SupplierID;    // Koniecznie ustawiamy identyfikator !!
            editDto.ProductID = SelectedProduct.ProductID;     // Koniecznie ustawiamy identyfikator !!

            if (IsValidNewDtoOf(editDto))                       // Walidacja danych wejœciowych
            {
                _appService.UpdateProductUsingMapped(editDto);  // Ta ga³¹Ÿ zadzia³a, jeœli walidacja przejdzie pomyœlnie
                _logger.LogInformation($"Product {SelectedProduct.ProductName} (Id={SelectedProduct.ProductName})");

                // Ponowne odœwie¿enie zawartoœci tabeli
                ReLoadingGridData(SelectedProduct.SupplierID);

                _dialogService.ShowMessageBox(this,
                    $"Existing product {productName} has been successfully updated", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _logger.LogWarning($"Validation failed when editing product with Id={SelectedProduct.ProductID}");
                _dialogService.ShowMessageBox(this,       // A ta ga³¹Ÿ - jeœli dane bêd¹ nieprawid³owe
                    "Invalid source data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            _logger.LogWarning("Edit product: no row selected for editing");
            _dialogService.ShowMessageBox(this,       // Wyœwietli siê, jeœli u¿ytkownik nie wybra³ niczego z tabeli
                "You must first select a row to edit!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

    }

    /// <summary>
    /// Logika usuwania
    /// </summary>
    /// <param name="supplier"></param>
    private async void OnDeleteProductAsync(ItemProductGridDTO product)
    {
        if (product == null)
        {
            _logger.LogWarning("Delete product: null was passed");
            return;
        }
        _appService.DeleteProductUsingMapped(product.ProductID);
        _logger.LogInformation($"Attempting to delete product {product.ProductName} (Id={product.ProductID})");

        // Ponowne odœwie¿enie zawartoœci tabeli
        ReLoadingGridData(SelectedItem.Id);

        // Na koniec komunikat dialogowy o pomyœlnym usuniêciu
        _dialogService.ShowMessageBox(this,
            $"Deleted product {product.ProductName}",
            "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    #endregion

    #region Methods

    private void ReLoadingGridData(int idValue)
    {
        _logger.LogInformation("Refreshing the contents of the product table");

        IEnumerable<ItemProductGridDTO> productsCollection = _appService.GetProductsCollectionUsing(idValue);
        if (!productsCollection.Any())
        {
            _logger.LogError("Error loading product data: collection is empty");
            _dialogService.ShowMessageBox(this,
                "Error loading product data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Konwertujemy na ObservableCollection dla WPF
        ItemsGrid = new ObservableCollection<ItemProductGridDTO>(productsCollection);
    }

    private bool IsValidNewDtoOf(ItemProductGridDTO oDto)
    {
        // Mo¿na dodaæ logowanie przy rzeczywistej walidacji
        return true; // TODO : Uzupe³niæ póŸniej rzeczywist¹ logik¹ sprawdzania danych, na razie zostaje tak
    }

    #endregion
}