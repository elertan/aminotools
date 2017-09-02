using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AminoTools.Exceptions;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace AminoTools
{
    public class SettingsManager
    {
        public static void LogCrashReport(string msg)
        {
            SaveSettingAsync(AvailableSettings.ExceptionMessage, msg).RunSynchronously();
        }

        public static async Task ClearSettingAsync(AvailableSettings key)
        {
            if (Application.Current.Properties.ContainsKey(key.ToString()))
                Application.Current.Properties.Remove(key.ToString());
            await Application.Current.SavePropertiesAsync();
        }

        public static async Task SaveSettingAsync(AvailableSettings key, object data)
        {
            if (Application.Current.Properties.ContainsKey(key.ToString()))
                Application.Current.Properties[key.ToString()] = data;
            else Application.Current.Properties.Add(key.ToString(), data);

            await Application.Current.SavePropertiesAsync();
        }

        public static async Task SaveComplexSettingAsync<T>(AvailableSettings key, T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            await SaveSettingAsync(key, json);
        }

        public static T GetSetting<T>(AvailableSettings key)
        {
            if (!Application.Current.Properties.ContainsKey(key.ToString()))
                throw new SettingDoesNotExistException(key.ToString());
            var s = Application.Current.Properties[key.ToString()] as string;
            if (s != null && Helpers.IsValidJson(s))
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            return (T) Application.Current.Properties[key.ToString()];
        }

        public static T GetComplexSetting<T>(AvailableSettings key)
        {
            var json = GetSetting<string>(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static T GetSettingWithFallback<T>(AvailableSettings key, T fallback)
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

        public static T GetSettingOrDefault<T>(AvailableSettings key)
        {
            var val = key.ToString();
            try
            {
                return GetSetting<T>(key);
            }
            catch (SettingDoesNotExistException)
            {
                return default(T);
            }
        }

        public enum AvailableSettings
        {
            Account,
            Username,
            ExceptionMessage
        }
    }
}
