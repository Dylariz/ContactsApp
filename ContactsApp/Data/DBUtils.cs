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
    /// Prepares the database to work with the application, reset default tables
    /// </summary>
    public static void PrepareDatabase()
    {
        using var db = GetContext();
        
        if (!db.Database.EnsureCreated())
        {
            // Delete constants and temporary data
            db.Genders.RemoveRange(db.Genders);
            db.Sessions.RemoveRange(db.Sessions);
            
            // Reset autoincrement of removed tables
            db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Genders' OR name = 'Sessions'");
        }
        
        // Set default data
        var appConfig = AppConfig.GetInstance();
        db.Genders.AddRange(appConfig.Genders.Select(x => new Gender {Type = x}));
        
        db.SaveChanges();
    }
}