namespace ContactsApp.Models;

public class User
{
    public int Id { get; init; }
    public required string Login { get; init; }
    public required string PasswordSalt { get; init; }
    public required string PasswordHash { get; init; }
    public int ProfileId { get; init; }
    public required Profile Profile { get; init; }
}