using System.IO;
using System.Text.Json;

namespace axion.Utils;

public static class Storage
{
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "axion.json");

    private static readonly Dictionary<string, object> Cache;



    static Storage()
    {
        Cache = File.Exists(FilePath)
            ? JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(FilePath))!
            : new Dictionary<string, object>();
    }



    public static void Set(string key, object value)
    {
        Cache[key] = value;
        File.WriteAllText(FilePath, JsonSerializer.Serialize(Cache));
    }

    public static T Get<T>(string key)
    {
        return Cache.TryGetValue(key, out var v)
            ? (T) v
            : default!;
    }

    public static bool Contains(string key)
    {
        return Cache.ContainsKey(key);
    }

    public static bool TryGet<T>(string key, out T? value)
    {
        var result = Cache.TryGetValue(key, out var v);
        value = result
            ? (T)v!
            : default!;

        return result;
    }
}