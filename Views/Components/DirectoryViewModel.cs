using System.IO;

namespace axion.Views.Components;

public class DirectoryViewModel : ViewModel, IEntry
{
    public const string MetaFileExtension = "meta";



    private string _name = "";

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string EntryName
    {
        get => Name;
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

    private DirectoryViewModel? _parent;
    public DirectoryViewModel Parent => _parent ??= new DirectoryViewModel(Directory.GetParent(EntryPath)!.ToString());


    public DirectoryViewModel()
    {
    }

    public DirectoryViewModel(string path) : this()
    {
        _name = System.IO.Path.GetFileName(path);
        _path = path;
    }



    public IEnumerable<DirectoryViewModel> GetDirectories()
    {
        return Directory.GetDirectories(Path)
            .Select(directory => new DirectoryViewModel(directory));
    }

    public IEnumerable<TimerViewModel> GetTimers()
    {
        return Directory.GetFiles(Path)
            .Where(TimerViewModel.IsFileTimer)
            .Select(timer => new TimerViewModel(timer));
    }



    private TimeSpan? _elapsed;

    public TimeSpan EntryTimeElapsed
    {
        get
        {
            if (_elapsed != null)
            {
                return _elapsed ?? TimeSpan.Zero;
            }

            _elapsed = GetDirectories()
                .Aggregate(TimeSpan.Zero, (current, directory) =>
                    current + directory.EntryTimeElapsed);

            _elapsed += GetTimers()
                .Aggregate(TimeSpan.Zero, (current, file) =>
                    current + file.EntryTimeElapsed);

            return _elapsed ?? TimeSpan.Zero;
        }
    }

    private TimeSpan? _max;

    public TimeSpan Max => _max ??= TimeSpan.FromSeconds(
            GetTimers()
                .Max(x => x.EntryTimeElapsed.TotalSeconds));

    private TimeSpan? _min;

    public TimeSpan Min => _min ??= TimeSpan.FromSeconds(
        GetTimers()
            .Min(x => x.EntryTimeElapsed.TotalSeconds));

    private TimeSpan? _average;

    public TimeSpan Average => _average ??= TimeSpan.FromSeconds(
        GetTimers()
            .Average(x => x.EntryTimeElapsed.TotalSeconds));

    private int? _count;

    public int Count => _count ??= GetTimers().Count();



    public bool Rename(string name)
    {
        var parent = Directory.GetParent(EntryPath);
        if (parent == null)
        {
            return false;
        }

        Directory.Move(EntryPath, System.IO.Path.Join(parent.ToString(), name));
        return true;
    }

    public bool Delete()
    {
        Directory.Delete(EntryPath, true);
        return true;
    }
}