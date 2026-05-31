namespace VitrineSemiJoias.Services.Interfaces;

public interface IEmailSenderService
{
	Task SendPasswordResetAsync(string email, string resetLink, CancellationToken cancellationToken = default);

}