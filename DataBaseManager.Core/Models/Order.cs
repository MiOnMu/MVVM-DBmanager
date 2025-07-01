using System;

namespace DataBaseManager.Core.Models;

[Serializable]
public class Order
{
    /// <summary>
    /// Identyfikator
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Nazwa 
    /// </summary>
    public string Name { get; set; }
}