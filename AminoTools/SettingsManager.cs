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
        private const string JsonSerializedData = "__JSONSTRINGDATA__";

        public static async Task ClearSettingAsync(string key)
        {
            if (Application.Current.Properties.ContainsKey(key)) Application.Current.Properties.Remove(key);
            await Application.Current.SavePropertiesAsync();
        }

        public static async Task SaveSettingAsync(string key, object data)
        {
            if (Application.Current.Properties.ContainsKey(key)) Application.Current.Properties[key] = data;
            else Application.Current.Properties.Add(key, data);

            await Application.Current.SavePropertiesAsync();
        }

        public static async Task SaveComplexSettingAsync<T>(string key, T obj)
        {
            key = key + JsonSerializedData;
            var json = JsonConvert.SerializeObject(obj);
            await SaveSettingAsync(key, json);
        }

        public static T GetSetting<T>(string key)
        {
            if (!Application.Current.Properties.ContainsKey(key)) throw new SettingDoesNotExistException(key);
            return (T)Application.Current.Properties[key];
        }

        public static T GetComplexSetting<T>(string key)
        {
            key = key + JsonSerializedData;
            var json = GetSetting<string>(key);
            return JsonConvert.DeserializeObject<T>(json);
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
