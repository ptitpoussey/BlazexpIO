using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientLib.Helpers
{
    public class SerializeHelper
    {
        public static string Serialize<T>(T obj) => JsonSerializer.Serialize(obj);
        public static T DeserializeJsonString<T>(string json) => JsonSerializer.Deserialize<T>(json);
        public static IList<T> DeserializeJsonStringList<T>(string json) => JsonSerializer.Deserialize<IList<T>>(json);
    }
}
