namespace ContactsApp.Models;

public class Contact
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public User? User { get; init; }
    public required int FamiliarId { get; init; }
    public Profile? Familiar { get; init; }
}