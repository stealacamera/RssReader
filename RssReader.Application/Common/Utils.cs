using System.Text.RegularExpressions;

namespace RssReader.Application.Common;

internal static class Utils
{
    public static int UsernameMaxLength = 100;

    public static bool IsPasswordValid(string password)
    {
        Regex allowedCharacters = new(@"^(?=.*[a-z])(?=.*\d)(?=.*[^\da-zA-Z]).*$");
        
        return password.Length >= 8 && 
               password.Length <= 30 && 
               allowedCharacters.IsMatch(password);
    }
}
