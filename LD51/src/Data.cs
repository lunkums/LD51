using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            string value = data.GetValueOrDefault(propertyName, "");
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
