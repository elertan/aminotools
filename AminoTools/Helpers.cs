using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AminoTools.Exceptions;
using Xamarin.Forms;

namespace AminoTools
{
    public static class Helpers
    {
        public static bool IsValidEmail(string emailAddress)
        {
            if (String.IsNullOrWhiteSpace(emailAddress)) return false;

            var match = Regex.Match(emailAddress, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            return match.Success;
        }
    }
}
