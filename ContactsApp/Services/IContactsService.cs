﻿using ContactsApp.Models;

namespace ContactsApp.Services;

public interface IContactsService
{
    Task<IEnumerable<Profile>> GetUserContactsFullAsync(string sessionToken, CancellationToken cancellationToken);

    Task<IEnumerable<int>> GetUserContactsIdsAsync(string sessionToken, CancellationToken cancellationToken);
    Task AddContactAsync(string sessionToken, int contactId, CancellationToken httpContextRequestAborted);
}