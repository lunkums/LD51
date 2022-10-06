using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LD51
{
    public class Data
    {
        private static Dictionary<string, string> data = new Dictionary<string, string>();
        private static string dataFilePath = "settings/settings.json";

        static Data()
        {
            data = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(dataFilePath));
        }

        // "where T : struct" should limit T to primitive types
        public static T Get<T>(string propertyName) where T : struct
        {
            string value = GetRaw(propertyName);
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
        
        public static string GetRaw(string propertyName)
        {
            return data.GetValueOrDefault(propertyName, "");
        }
    }
}
