using ContactsApp.Data;
using ContactsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Services;

public class ContactsService : IContactsService
{
    private readonly IAuntificationService _auth;

    public ContactsService(IAuntificationService service)
        => _auth = service;

    public async Task AddContactAsync(string sessionToken, int contactId, CancellationToken httpContextRequestAborted)
    {
        if (await _auth.SessionIsValid(sessionToken) == false)
            throw new ArgumentException("Invalid session token");

        var db = DBUtils.GetContext();
        var session = await db.Sessions.Include(session => session.User).FirstOrDefaultAsync(x => x.Token == sessionToken, httpContextRequestAborted);

        var contactProfile = await db.Profiles.FirstOrDefaultAsync(x => x.Id == contactId, httpContextRequestAborted);
        if (contactProfile == null)
            throw new ArgumentException("User not found");

        var existingContact =
            await db.Contacts.FirstOrDefaultAsync(x => x.UserId == session!.UserId && x.FamiliarId == contactId, httpContextRequestAborted);
        if (existingContact != null)
            throw new ArgumentException("Contact already exists");

        var newContact = new Contact
        {
            UserId = session!.UserId,
            FamiliarId = contactProfile.Id
        };

        await db.Contacts.AddAsync(newContact, httpContextRequestAborted);
        await db.SaveChangesAsync(httpContextRequestAborted);
    }
    
    public async Task RemoveContactAsync(string sessionToken, int contactId, CancellationToken httpContextRequestAborted)
    {
        if (await _auth.SessionIsValid(sessionToken) == false)
            throw new ArgumentException("Invalid session token");

        var db = DBUtils.GetContext();
        var session = await db.Sessions.Include(session => session.User).FirstOrDefaultAsync(x => x.Token == sessionToken, httpContextRequestAborted);

        var contact = await db.Contacts.FirstOrDefaultAsync(x => x.UserId == session!.UserId && x.FamiliarId == contactId, httpContextRequestAborted);
        if (contact == null)
            throw new ArgumentException("Contact not found");

        db.Contacts.Remove(contact);
        await db.SaveChangesAsync(httpContextRequestAborted);
    }

    public async Task<IEnumerable<Profile>> GetUserContactsFullAsync(string sessionToken,
        CancellationToken cancellationToken)
    {
        if (await _auth.SessionIsValid(sessionToken) == false)
            throw new ArgumentException("Invalid session token");

        var db = DBUtils.GetContext();
        var session = await db.Sessions.Include(session => session.User).FirstOrDefaultAsync(x => x.Token == sessionToken, cancellationToken);

        var contacts = db.Contacts.Where(x => x.UserId == session!.UserId);
        var familiarProfiles = new List<Profile>();

        foreach (var familiar in contacts.Select(x => x.Familiar))
        {
            if (familiar == null)
                continue;

            familiarProfiles.Add(familiar);
        }

        return familiarProfiles;
    }

    public async Task<IEnumerable<int>> GetUserContactsIdsAsync(string sessionToken,
        CancellationToken cancellationToken)
    {
        if (await _auth.SessionIsValid(sessionToken) == false)
            throw new ArgumentException("Invalid session token");

        var db = DBUtils.GetContext();
        var session = await db.Sessions.FirstOrDefaultAsync(x => x.Token == sessionToken, cancellationToken);

        var contacts = db.Contacts.Where(x => x.UserId == session!.UserId);
        return contacts.Select(x => x.FamiliarId);
    }
}