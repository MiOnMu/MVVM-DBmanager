namespace DataBaseManager.DataAccess.Contracts.DTOs;

/// <summary>
/// Prywatny obiekt DTO, powiązany z konkretną tabelą (Customers).
/// Potrzebny do wyświetlania elementów w tabeli klientów.
/// </summary>
public class ItemCustomerGridDTO
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Imię i nazwisko kontaktowe klienta, wyświetlane na liście
    /// </summary>
    public string ContactName { get; set; }

    /// <summary>
    /// Telefon
    /// </summary>
    public string CustomerPhone { get; set; }

    /// <summary>
    /// Adres e-mail
    /// </summary>
    public string CustomerEmail { get; set; }
}