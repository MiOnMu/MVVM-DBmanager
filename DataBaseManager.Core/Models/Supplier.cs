using System;

namespace DataBaseManager.Core.Models;

[Serializable]
public class Supplier
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int SupplierId { get; set; }

    /// <summary>
    /// Nazwa, wyświetlana na liście
    /// </summary>
    public string CompanyName { get; set; }

    /// <summary>
    /// Nazwa kontaktowa, wyświetlana na liście
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
}