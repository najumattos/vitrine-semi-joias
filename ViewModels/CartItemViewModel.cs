namespace VitrineSemiJoias.ViewModels;

public class CartItemViewModel
{
    public int ProductId { get; set; }
    public int JewelryCode { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
}