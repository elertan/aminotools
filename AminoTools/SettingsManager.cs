using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Exceptions;
using Xamarin.Forms;

namespace AminoTools
{
    public class SettingsManager
    {
        public static async Task SaveSetting(string key, object data)
        {
            if (Application.Current.Properties.ContainsKey(key)) Application.Current.Properties[key] = data;
            else Application.Current.Properties.Add(key, data);

            await Application.Current.SavePropertiesAsync();
        }

        public static T GetSetting<T>(string key)
        {
            if (!Application.Current.Properties.ContainsKey(key)) throw new SettingDoesNotExistException(key);
            return (T)Application.Current.Properties[key];
        }

        public static T GetSettingWithFallback<T>(string key, T fallback)
        {
            try
            {
                return GetSetting<T>(key);
            }
            catch (SettingDoesNotExistException)
            {
                return fallback;
            }
        }
    }
}
