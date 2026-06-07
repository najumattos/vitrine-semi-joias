using System.ComponentModel.DataAnnotations;

namespace VitrineSemiJoias.ViewModels;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Email e obrigatorio.")]
    [EmailAddress(ErrorMessage = "Email em formato invalido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Token e obrigatorio.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nova senha e obrigatoria.")]
    [StringLength(100, ErrorMessage = "A {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmacao de senha e obrigatoria.")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "As senhas nao conferem.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
