using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Models;

public class Profile
{
    public int Id { get; init; }
    public required string FullName { get; init; }
    public string? NickName { get; init; }
    public string? BirthDate { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public int? GenderId { get; init; }
    public Gender? Gender { get; init; }
}