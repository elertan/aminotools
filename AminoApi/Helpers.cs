using System;
using System.Text.RegularExpressions;

namespace AminoApi
{
    public static class Helpers
    {
        public static bool IsValidEmail(string emailAddress)
        {
            if (String.IsNullOrWhiteSpace(emailAddress)) return false;

            var match = Regex.Match(emailAddress, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return match.Success;
        }

        public static int GetUnixTimeStamp()
        {
            return (Int32) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
