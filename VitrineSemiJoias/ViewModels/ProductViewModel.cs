using System.ComponentModel.DataAnnotations;
using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O código da peça é obrigatório.")]
    public int JewelryCode { get; set; }

    [Required(ErrorMessage = "O título da peça é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório.")]
    [Range(0.01, 999999.99, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Price { get; set; }

    [Display(Name = "URL da Imagem")]
    public string ImageUrl { get; set; } = string.Empty;   

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public CategoryEnum CategoryEnum { get; set; }

    public bool IsAvailable { get; set; }
}
