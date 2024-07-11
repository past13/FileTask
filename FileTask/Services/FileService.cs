using System.Diagnostics;
using FileTask.Entities;
using FileTask.Services.Interfaces;

namespace FileTask.Services;

public class FileService : IFileService
{
    public async Task<List<string>> ValidateAccountsAsync(Stream stream, Action<TimeSpan, int> measureTimeCallback)
    {
        var invalidLines = new List<string>();
        var lineNumber = 0;

        using var reader = new StreamReader(stream);
        while (reader.Peek() >= 0)
        {
            lineNumber++;
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(' ');
            if (parts.Length != 2)
            {
                invalidLines.Add($"Invalid format - not enough data for line {lineNumber}: '{line}'");
                continue;
            }

            var account = new Account(parts[0], parts[1]);
            
            var stopwatch = Stopwatch.StartNew();
            
            if (!account.IsValid(out var errors))
            {
                invalidLines.Add($"{string.Join(", ", errors)} - not valid for line {lineNumber} '{line}'");
            }
            
            stopwatch.Stop();
            var lineValidationTime = stopwatch.Elapsed;
            measureTimeCallback?.Invoke(lineValidationTime, lineNumber);
        }

        return invalidLines;
    }
}