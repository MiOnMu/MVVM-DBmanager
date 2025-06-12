namespace DataBaseManager.DataAccess.Contracts.DTOs;

/// <summary>
/// Dedykowany obiekt DTO (Data Transfer Object) dla encji Customers.
/// Potrzebny do wyświetlania elementów w tabeli (siatce) klientów.
/// </summary>
public class ItemCustomerGridDTO
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nazwa kontaktowa klienta, wyświetlana na liście
    /// </summary>
    public string ContactName { get; set; }

    /// <summary>
    /// Telefon
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Adres e-mail
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Adres geograficzny
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Miasto
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Region
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// Kod pocztowy
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    /// Kraj
    /// </summary>
    public string Country { get; set; }

}