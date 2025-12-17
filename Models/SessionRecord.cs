using System.Globalization;

namespace axion.Models;

public record SessionRecord(DateTime Timestamp, int Duration)
{
    private const string Separator = " ";

    public static SessionRecord From(string line)
    {
        var parts = line
            .Trim()
            .Split(Separator);

        if (parts.Length != 2)
        {
            throw new Exception($"Expected two parts for session record. [DateTime]{Separator}[int]");
        }

        var timestamp = DateTime.ParseExact(
            parts[0],
            "O",
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind
        );

        var duration = int.Parse(parts[1]);

        return new SessionRecord(timestamp, duration);
    }



    public string Template => ToString();

    public override string ToString()
    {
        return Timestamp.ToString("O", CultureInfo.InvariantCulture) + Separator + Duration;
    }
}