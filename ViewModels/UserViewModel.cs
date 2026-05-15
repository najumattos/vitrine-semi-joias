using System.ComponentModel.DataAnnotations;
using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O email é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O email deve ter entre 3 e 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "A senha deve ter entre 3 e 100 caracteres.")]
    public string Password { get; set; } = string.Empty;
}
