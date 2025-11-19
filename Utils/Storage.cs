using System.IO;
using System.Text.Json;

namespace axion.Utils;

public static class Storage
{
    private static readonly string FilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "axion.json");

    private static Dictionary<string, object> _cache;



    static Storage()
    {
        _cache = ReadFile();

        Console.WriteLine();
        Console.WriteLine(FilePath);
        Console.WriteLine();
    }



    private static Dictionary<string, object> ReadFile()
    {
        return File.Exists(FilePath)
            ? JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(FilePath))!
            : new Dictionary<string, object>();
    }

    public static void Set(string key, object value)
    {
        _cache[key] = value;
        File.WriteAllText(FilePath, JsonSerializer.Serialize(_cache));
        _cache = ReadFile();
    }

    private static JsonElement? Get(string key)
    {
        return _cache.TryGetValue(key, out var v)
            ? (JsonElement) v
            : null;
    }

    public static T? Get<T>(string key)
    {
        var element = Get(key);
        return element == null
            ? default
            : element.Value.Deserialize<T>();
    }

    public static bool Contains(string key)
    {
        return _cache.ContainsKey(key);
    }

    public static bool TryGet<T>(string key, out T? value)
    {
        var result = _cache.TryGetValue(key, out var v);
        value = result
            ? ((JsonElement) v!).Deserialize<T>()
            : default!;

        return result;
    }
}