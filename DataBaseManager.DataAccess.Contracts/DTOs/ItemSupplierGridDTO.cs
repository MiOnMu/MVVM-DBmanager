namespace DataBaseManager.DataAccess.Contracts.DTOs;

/// <summary>
/// Prywatny obiekt DTO, powiązany ze specyfiką dostawcy (Supplier).
/// Potrzebny do wyświetlania elementów w tabeli dostawców.
/// </summary>
public class ItemSupplierGridDTO
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int SupplierId { get; set; }

    /// <summary>
    /// Nazwa wyświetlana na liście
    /// </summary>
    public string CompanyName { get; set; }

    /// <summary>
    /// Imię i nazwisko kontaktowe, wyświetlane na liście
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
}