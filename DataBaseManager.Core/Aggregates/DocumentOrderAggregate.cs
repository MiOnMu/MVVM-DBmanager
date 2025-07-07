using DataBaseManager.Core.Models;

namespace DataBaseManager.Core.Aggregates;

[Serializable]
public class DocumentOrderAggregate
{
    #region Properties

    /// <summary>
    /// Unikalny identyfikator agregatu
    /// </summary>
    public Guid AggregateId { get; } = Guid.NewGuid();

    /// <summary>
    /// "Nagłówek" dokumentu
    /// </summary>
    public Order HeaderDocument { get; set; }

    /// <summary>
    /// "Część tabelaryczna" dokumentu
    /// </summary>
    public List<OrderDetails> OrderDetails { get; set; }
    #endregion

    #region Ctors
    public DocumentOrderAggregate()
    {
        // Obowiązkowa inicjalizacja elementów-obiektów
        HeaderDocument = new Order();
        OrderDetails = new List<OrderDetails>();
    }
    #endregion

}