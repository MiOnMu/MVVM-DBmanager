namespace DataBaseManager.DataAccess.Contracts.DTOs;

public class ItemProductGridDTO
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public int SupplierID { get; set; }
    public decimal UnitPrice { get; set; }
    public bool Discontinued { get; set; }
}