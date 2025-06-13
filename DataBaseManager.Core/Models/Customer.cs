using System;

namespace DataBaseManager.Core.Models;

[Serializable]
public class Customer
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

    #region Domain Specific Data
    /// <summary>
    /// Dynamicznie przypisywana ranga klienta
    /// </summary>
    public int CustomerRank { get; set; }

    /// <summary>
    /// Specyficzne informacje dotyczące użytkownika
    /// </summary>
    public string CustomerSpecificData { get; set; }
    #endregion
}