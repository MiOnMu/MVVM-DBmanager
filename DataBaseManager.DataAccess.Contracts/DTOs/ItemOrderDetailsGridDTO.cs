namespace DataBaseManager.DataAccess.Contracts.DTOs;

public class ItemOrderDetailsGridDTO
{
    public int OrderDetailID { get; set; }
    public int OrderId { get; set; }
    public int ProductID { get; set; } = 0;
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
}