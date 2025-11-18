using axion.Utils;

namespace axion.ViewModels;

public class DirectoryViewModel : ViewModel, IEntry
{
    private string _name = "";

    public string EntryName
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Name
    {
        get => EntryName;
        set => EntryName = value;
    }


    private string _path = "";

    public string EntryPath
    {
        get => _path;
        set => SetProperty(ref _path, value);
    }

    public string Path
    {
        get => EntryPath;
        set => EntryPath = value;
    }
}