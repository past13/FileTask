namespace FileTask.Services.Interfaces;

public interface IFileService
{
    Task<List<string>> ValidateAccountsAsync(Stream stream, Action<TimeSpan, int> measureTimeCallback);
}