using System.Text.Json;

namespace BestlaArquitectureApplicationCore.Extentions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            MaxDepth = 0,
            IgnoreNullValues = true,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true
        };
        
        public static T FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json, _jsonOptions);

        public static string ToJson<T>(this T obj) =>
            JsonSerializer.Serialize<T>(obj, _jsonOptions);
    }
}