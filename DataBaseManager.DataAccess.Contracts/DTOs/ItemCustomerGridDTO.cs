namespace DataBaseManager.DataAccess.Contracts.DTOs;

/// <summary>
/// Prywatny obiekt DTO, powiązany ze specyfiką klienta (Customer).
/// Potrzebny do wyświetlania elementów w tabeli klientów.
/// </summary>
public class ItemCustomerGridDTO
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Nazwa wyświetlana na liście
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Imię i nazwisko kontaktowe klienta, wyświetlane na liście
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