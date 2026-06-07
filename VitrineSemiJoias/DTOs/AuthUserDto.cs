namespace VitrineSemiJoias.DTOs;

public class AuthUserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Profile { get; set; }
}
