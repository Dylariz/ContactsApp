using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsApp.Models;

public class Gender
{
    public int Id { get; init; }
    public required string Type { get; init; }
}