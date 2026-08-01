using System.Net;
using Captain.Models;
using Microsoft.AspNetCore.Identity;

namespace Captain.Services;

public class FakeConfirmEmailSender : IEmailSender<AppUser>
{
  private readonly ILogger _logger;

  public FakeConfirmEmailSender(ILogger<FakeConfirmEmailSender> logger)
  {
    _logger = logger;
  }

  public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
  {
    var decodedLink = WebUtility.HtmlDecode(confirmationLink);

    _logger.LogInformation(
      """
      ===================================
      CONFIRMATION EMAIL

      To: {Email}

      Confirmation Link:

      {Link}

      ===================================
      """,
      email,
      decodedLink
    );

    return Task.CompletedTask;
  }

  public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
  {
    _logger.LogInformation("Reset code: " + resetCode);
    return Task.CompletedTask;
  }

  public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
  {
    _logger.LogInformation("Reset link: " + resetLink);
    return Task.CompletedTask;
  }
}
