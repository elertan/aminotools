using System;

namespace AminoTools.Exceptions
{
    public class SettingDoesNotExistException : Exception
    {
        public string Key { get; }

        public SettingDoesNotExistException(string key)
        {
            Key = key;
        }
    }
}