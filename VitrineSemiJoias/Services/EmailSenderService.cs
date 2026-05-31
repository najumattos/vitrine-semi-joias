using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using VitrineSemiJoias.Configurations;
using VitrineSemiJoias.Services.Interfaces;

namespace VitrineSemiJoias.Services;

public class EmailSenderService(
    IOptions<SmtpOptions> options,
    ILogger<EmailSenderService> logger) : IEmailSenderService
{
	private readonly SmtpOptions smtpOptions = options.Value;

	public async Task SendPasswordResetAsync(string email, string resetLink, CancellationToken cancellationToken = default)
	{
		var message = new MimeMessage();
		message.From.Add(new MailboxAddress(smtpOptions.SenderName, smtpOptions.SenderEmail));
		message.To.Add(MailboxAddress.Parse(email));
		message.Subject = "Redefinicao de senha";

		var builder = new BodyBuilder
		{
			TextBody = $"Para redefinir sua senha, acesse: {resetLink}",
			HtmlBody = $"<p>Para redefinir sua senha, clique no link abaixo:</p><p><a href=\"{resetLink}\">Redefinir senha</a></p>"
		};

		message.Body = builder.ToMessageBody();

		try
		{
			using var client = new SmtpClient();
			await client.ConnectAsync(smtpOptions.Server, smtpOptions.Port, SecureSocketOptions.StartTls, cancellationToken);
			await client.AuthenticateAsync(smtpOptions.Username, smtpOptions.Password, cancellationToken);
			await client.SendAsync(message, cancellationToken);
			await client.DisconnectAsync(true, cancellationToken);

			logger.LogInformation("Email de redefinicao enviado para {Email}", email);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Falha ao enviar email de redefinicao para {Email}", email);
			throw;
		}
	}
}