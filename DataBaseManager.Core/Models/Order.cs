namespace DataBaseManager.Core.Models;

[Serializable]
public class Order
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int OrderId { get; set; }


    /// <summary>
    /// Customer Identyfikator
    /// </summary>
    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime RequiredDate { get; set; }
    public string OrderNumber { get; set; }

    public DateTime ShippedDate { get; set; }

    public int ShipVia { get; set; }

    public string ShipName { get; set; }

    public string ShipRegion { get; set; }

    public string ShipAddress { get; set; }

    public string ShipCity { get; set; }

    public string ShipCountry { get; set; }

    public string ShipPostalCode { get; set; }

    #region Domain Specific Data
    /// <summary>
    /// Dynamicznie przypisywana ranga ordera
    /// </summary>
    public int OrderRank { get; set; }


    public decimal SummaryValue { get; set; }

    /// <summary>
    /// Specyficzne informacje dotycz¹ce u¿ytkownika
    /// </summary>
    public string OrderSpecificData { get; set; }
    #endregion

}