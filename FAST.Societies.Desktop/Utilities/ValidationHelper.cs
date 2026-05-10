namespace FAST.Societies.Desktop.Utilities;
using System.Text.RegularExpressions;

public static class ValidationHelper
{
    public static bool Required(params string[] values) => values.All(v => !string.IsNullOrWhiteSpace(v));

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Uses a basic structure regex. In production, consider MailAddress or more comprehensive regex.
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return emailRegex.IsMatch(email);
    }

    public static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        
        // Password must be at least 6 characters, contain at least one digit and one letter.
        return password.Length >= 6 && 
               password.Any(char.IsDigit) && 
               password.Any(char.IsLetter);
    }
}

