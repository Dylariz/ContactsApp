using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Data;

public sealed class ApplicantionContext : DbContext
{
    public ApplicantionContext(DbContextOptions<ApplicantionContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Gender> Genders { get; set; }
    public DbSet<Session> Sessions { get; set; }
}