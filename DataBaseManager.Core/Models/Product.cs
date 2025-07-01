using System;

namespace DataBaseManager.Core.Models;

[Serializable]
public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public int SupplierID { get; set; }
    public decimal UnitPrice { get; set; }
    public bool Discontinued { get; set; }
    public int ProductRank { get; set; }
    public string ProductSpecificData { get; set; }
}