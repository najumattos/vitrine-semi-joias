using VitrineSemiJoias.Common;
using VitrineSemiJoias.DTOs;

namespace VitrineSemiJoias.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Realiza a autenticação do usuário no sistema utilizando o ASP.NET Core Identity.
    /// </summary>
    /// <param name="loginDto">Objeto contendo as credenciais de e-mail, senha e persistência.</param>
    /// <returns>Um objeto Result contendo os dados do usuário autenticado ou a mensagem de falha.</returns>
    Task<Result<AuthUserDto>> ValidateUserAsync(LoginDto loginDto);

    /// <summary>
    /// Processa a solicitacao de esqueci minha senha, gerando token e enviando email.
    /// </summary>
    /// <param name="dto">Objeto contendo o email do usuario.</param>
    /// <returns>Um objeto Result indicando sucesso ou falha.</returns>
    Task<Result> ProcessForgotPasswordAsync(ForgotPasswordDto dto);

    /// <summary>
    /// Processa a redefinicao de senha do usuario.
    /// </summary>
    /// <param name="dto">Objeto contendo email, token e nova senha.</param>
    /// <returns>Um objeto Result indicando sucesso ou falha.</returns>
    Task<Result> ProcessResetPasswordAsync(ResetPasswordDto dto);

    /// <summary>
    /// Encerra a sessão do usuário atual limpando os cookies de autenticação.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento para interromper a operação assíncrona se a requisição for cancelada.</param>
    Task Logout(CancellationToken cancellationToken = default);

    }
