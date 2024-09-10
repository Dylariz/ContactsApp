namespace ContactsApp.Services;

public interface IAuntificationService
{
    Task<string> LoginAsync(string username, string password, CancellationToken httpContextRequestAborted);

    Task RegisterAsync(string login, string password, string fullName, CancellationToken httpContextRequestAborted);
    
    Task DeleteUser(string sessionToken, CancellationToken httpContextRequestAborted);

    Task<bool> SessionIsValid(string token);
}