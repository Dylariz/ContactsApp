using System.Text;
using ContactsApp.Data;
using ContactsApp.Models;
using HashLib4CSharp.Base;
using HashLib4CSharp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Services;

public class AuntificationService : IAuntificationService
{
    public async Task<string> LoginAsync(string login, string password, CancellationToken httpContextRequestAborted)
    {
        var db = DBUtils.GetContext();
        var user = await db.Users.FirstOrDefaultAsync(x => x.Login == login, httpContextRequestAborted);
        
        if (user == null)
            throw new ArgumentException("Invalid login");

        // Generate hash
        var passwordHash = GenerateHash(password, user.PasswordSalt);
        if (passwordHash != user.PasswordHash)
            throw new ArgumentException("Invalid password");

        // Generate token
        var token = Guid.NewGuid().ToString();
        await db.Sessions.AddAsync(new Session
        {
            Token = token,
            UserId = user.Id,
            ExpirationDate = DateTime.Now.AddHours(1)
        }, httpContextRequestAborted);
        await db.SaveChangesAsync(httpContextRequestAborted);
        return token;
    }


    public async Task RegisterAsync(string login, string password, string fullName, CancellationToken httpContextRequestAborted)
    {
        var db = DBUtils.GetContext();
        var user = await db.Users.FirstOrDefaultAsync(x => x.Login == login, httpContextRequestAborted);
        if (user != null)
            throw new ArgumentException("User already exists");
        
        var profile = new Profile { FullName = fullName };

        var salt = Guid.NewGuid().ToString();
        var passwordHash = GenerateHash(password, salt);
        await db.Users.AddAsync(new User
        {
            Login = login,
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            Profile = profile
        }, httpContextRequestAborted);
        await db.SaveChangesAsync(httpContextRequestAborted);
    }
    
    public async Task DeleteUser(string sessionToken, CancellationToken httpContextRequestAborted)
    {
        var db = DBUtils.GetContext();
        
        var session = await db.Sessions.Include(session => session.User).FirstOrDefaultAsync(x => x.Token == sessionToken, httpContextRequestAborted);
        if (session == null || await SessionIsValid(sessionToken) == false || session.User == null)
            throw new ArgumentException("Invalid session token");
        
        db.Users.Remove(session.User);
        await db.SaveChangesAsync(httpContextRequestAborted);
    }
    
    public async Task<bool> SessionIsValid(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;
        
        var db = DBUtils.GetContext();
        var session = await db.Sessions.FirstOrDefaultAsync(x => x.Token == token);
        
        if (session == null)
            return false;
        
        if (session.ExpirationDate < DateTime.Now)
        {
            db.Sessions.Remove(session);
            await db.SaveChangesAsync();
            return false;
        }
        
        return true;
    }

    private string GenerateHash(string password, string salt)
    {
        IHash hash = HashFactory.Crypto.CreateMD5();
        var passwordHash = hash.ComputeString(password + salt, Encoding.UTF8).ToString();
        return passwordHash;
    }
}