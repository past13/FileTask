using System.Text.RegularExpressions;

namespace FileTask.Entities;

public class Account(string firstName, string accountNumber)
{
    private string FirstName { get; } = firstName;
    private string AccountNumber { get; } = accountNumber;
    
    public bool IsValid(out List<string> errors)
    {
        errors = [];
        
        ValidateFirstName(errors);
        ValidateAccountNumber(errors);
        
        return errors.Count == 0;
    }
    
    private void ValidateFirstName(List<string> errors)
    {
        if (!Regex.IsMatch(FirstName, @"^[A-Z][a-zA-Z]*$"))
        {
            errors.Add("Account name");
        }
    }

    private void ValidateAccountNumber(List<string> errors)
    {
        if (!Regex.IsMatch(AccountNumber, @"^\d{7}$|^\d{7}p$") || !Regex.IsMatch(AccountNumber, @"^[34]"))
        {
            const string accountNumber = "account number";
            var message = errors.Count == 0 ? char.ToUpper(accountNumber[0]) + accountNumber[1..] : accountNumber;
            errors.Add(message);
        }
    }
}