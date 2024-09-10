using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Data;

public static class DBUtils
{
    public static ApplicantionContext GetContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicantionContext>();
        optionsBuilder.UseSqlite("Data Source=contacts.db");
        var instance = new ApplicantionContext(optionsBuilder.Options);
        return instance;
    }
    
    /// <summary>
    /// Prepares the database to work with the application
    /// </summary>
    public static void PrepareDatabase()
    {
        using var db = GetContext();
        if (db.Database.EnsureCreated())
        {
            InitDatabase(db);
        }
        else
        {
            db.Sessions.RemoveRange(db.Sessions);
        }
        
        db.SaveChanges();
    }
    
    /// <summary>
    /// Initializes the database with default values
    /// </summary>
    private static void InitDatabase(ApplicantionContext context)
    {
        List<Gender> defaultGenders =
        [
            new Gender { Type = "Male" },
            new Gender { Type = "Female" },
            new Gender { Type = "Helicopter" }
        ];
        
        context.Genders.AddRange(defaultGenders);
    }
}