using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Models;

public class Session
{
    public int Id { get; init; }
    public required int UserId { get; init; }
    public User? User { get; init; }
    public required string Token { get; init; }
    public required DateTime ExpirationDate { get; init; }
    
}