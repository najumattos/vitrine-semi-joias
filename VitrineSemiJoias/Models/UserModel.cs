using Microsoft.AspNetCore.Identity;
using VitrineSemiJoias.Enums;

namespace VitrineSemiJoias.Models;

public class UserModel : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;   
    public ProfileEnum Profile { get; set; }
}
