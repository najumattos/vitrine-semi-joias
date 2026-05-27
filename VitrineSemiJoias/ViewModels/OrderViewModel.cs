namespace VitrineSemiJoias.ViewModels;

public class OrderViewModel
{
    public IReadOnlyCollection<CartItemViewModel> Items { get; set; } = Array.Empty<CartItemViewModel>();
    public int TotalItems => Items.Count;
    public decimal TotalAmount => Items.Sum(item => item.Price);
}