using System.Collections;
using System.IO;

namespace axion.Utils;

public class FileSystem
{
    private static bool IsReservedDeviceName(string name)
    {
        var n = name.Split('.')[0].ToUpperInvariant();

        return n == "CON" || n == "PRN" || n == "AUX" || n == "NUL"
               || (n.StartsWith("COM") && int.TryParse(n.AsSpan(3), out var c1) && c1 is >= 1 and <= 9)
               || (n.StartsWith("LPT") && int.TryParse(n.AsSpan(3), out var c2) && c2 is >= 1 and <= 9);
    }

    public static bool IsValidFileName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (System.IO.Path.GetInvalidFileNameChars().Any(name.Contains))
        {
            return false;
        }

        if (name.EndsWith('.') || name.EndsWith(' '))
        {
            return false;
        }

        return !IsReservedDeviceName(name);
    }

    public static bool IsValidDirectoryName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        if (System.IO.Path.GetInvalidPathChars().Any(name.Contains))
        {
            return false;
        }

        if (System.IO.Path.GetInvalidFileNameChars().Any(name.Contains))
        {
            return false;
        }

        if (name.EndsWith('.') || name.EndsWith(' '))
        {
            return false;
        }

        return !IsReservedDeviceName(name);
    }
}