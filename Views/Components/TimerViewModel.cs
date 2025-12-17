using System.Collections.ObjectModel;
using System.IO;
using axion.Models;

namespace axion.Views.Components;

public class TimerViewModel() : ViewModel, IEntry
{
    private const string SeparatorProperty = ": ";
    public const string KeyElapsed = "elapsed";



    private string _name = "";

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string EntryName
    {
        get => System.IO.Path.GetFileNameWithoutExtension(Name);
        set => Name = value;
    }

    private string _path = "";

    public string Path
    {
        get => _path;
        set => SetProperty(ref _path, value);
    }

    public string EntryPath
    {
        get => Path;
        set => Path = value;
    }

    private Dictionary<string, string>? _properties;
    private ObservableCollection<SessionRecord>? _sessions;

    public TimeSpan Elapsed
    {
        get
        {
            if (_properties == null)
            {
                ReadFile();
            }

            return !_properties!.ContainsKey(KeyElapsed)
                ? TimeSpan.Zero
                : TimeSpan.FromSeconds(long.Parse(_properties![KeyElapsed]));
        }

        private set
        {
            if (_properties == null)
            {
                ReadFile();
            }

            _properties![KeyElapsed] = value.Seconds.ToString();
        }
    }

    public ObservableCollection<SessionRecord> Sessions
    {
        get
        {
            if (_sessions == null)
            {
                ReadFile();
            }

            return _sessions!;
        }
    }


    public TimerViewModel(string file) : this()
    {
        EntryPath = file;
        EntryName = System.IO.Path.GetFileName(file);
    }



    public bool Rename(string name)
    {
        var parent = Directory.GetParent(EntryPath);
        if (parent == null)
        {
            return false;
        }

        File.Move(EntryPath, System.IO.Path.Join(parent.ToString(), name + ".txt"));
        return true;
    }

    public bool Delete()
    {
        File.Delete(EntryPath);
        return true;
    }

    public void RecordSession(int duration)
    {
        RecordSession(DateTime.Now, duration);
    }

    public void RecordSession(DateTime timestamp, int duration)
    {
        if (_sessions == null)
        {
            ReadFile();
        }

        _sessions!.Add(new SessionRecord(timestamp, duration));
        Elapsed += TimeSpan.FromSeconds(duration);
        Save();
    }

    private void ReadFile()
    {
        _properties = new Dictionary<string, string>();
        _sessions = [];

        using var lines = File
            .ReadLines(Path)
            .GetEnumerator();

        while (lines.MoveNext())
        {
            var line = lines.Current.Trim();
            if (line == "")
            {
                break;
            }

            var pair = line.Split(SeparatorProperty);
            if (pair.Length != 2)
            {
                throw new Exception($"Expected property declaration. [property]{SeparatorProperty}[value]");
            }

            _properties[pair[0]] = pair[1];
        }

        while (lines.MoveNext())
        {
            var line = lines.Current.Trim();
            if (line == "")
            {
                break;
            }

            _sessions.Add(SessionRecord.From(line));
        }
    }

    public void Save()
    {
        if (_properties == null || _sessions == null)
        {
            return;
        }

        using var stream = new FileStream(
            Path,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None
        );

        using var writer = new StreamWriter(stream);

        foreach (var pair in _properties)
        {
            writer.WriteLine(pair.Key + SeparatorProperty + pair.Value);
        }

        writer.WriteLine();

        foreach (var session in _sessions)
        {
            writer.WriteLine(session);
        }
    }
}