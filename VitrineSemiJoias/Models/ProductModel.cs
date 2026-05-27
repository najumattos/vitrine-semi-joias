using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.Models;

public class ProductModel
{
    public int Id { get; set; }
    public int JewelryCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public CategoryEnum CategoryEnum { get; set; }
    public bool IsAvailable { get; set; }
}
