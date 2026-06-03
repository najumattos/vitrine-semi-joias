using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VitrineSemiJoias.Models;

namespace VitrineSemiJoias.Common;

public class UserClaimsPrincipalFactory(
    UserManager<UserModel> userManager,
    RoleManager<IdentityRole<int>> roleManager,
    IOptions<IdentityOptions> optionsAccessor)
    : UserClaimsPrincipalFactory<UserModel, IdentityRole<int>>(userManager, roleManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserModel user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("Profile", user.Profile.ToString()));
        return identity;
    }
}