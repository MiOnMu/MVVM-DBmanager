namespace DataBaseManager.CrossCutting.CommonDTOs;

/// <summary>
/// Ogólny obiekt DTO, niepowiązany z konkretną implementacją.
/// Potrzebny do wyświetlania elementów na liście rozwijanej
/// comboboxów.
/// </summary>
public class ItemDTO
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nazwa wyświetlana na liście
    /// </summary>
    public string Name { get; set; }
}