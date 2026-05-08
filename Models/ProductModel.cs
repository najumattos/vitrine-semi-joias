using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.Models;

public class ProductModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public CategoryEnum CategoryEnum { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public int StockQuantity { get; set; }
}
