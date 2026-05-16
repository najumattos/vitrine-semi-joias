using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public CategoryEnum CategoryEnum { get; set; }
    public bool IsAvailable { get; set; }
}
