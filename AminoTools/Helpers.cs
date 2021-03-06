﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AminoTools.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") // For object
                || strInput.StartsWith("[") && strInput.EndsWith("]")) // For array
            {
                try
                {
                    JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    //Exception in parsing json
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public static int GetUnixTimeStamp()
        {
            return (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}
