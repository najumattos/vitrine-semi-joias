using System.ComponentModel.DataAnnotations;

namespace VitrineSemiJoias.ViewModels;

public class ForgotPasswordViewModel
{
[Required(ErrorMessage = "Email e obrigatorio.")]
    [EmailAddress(ErrorMessage = "Email em formato invalido.")]

public string Email { get; set; } = string.Empty;
}