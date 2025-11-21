using System.IO;

namespace axion.Views.Components;

public class DirectoryViewModel : ViewModel, IEntry
{
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



    public void Rename(string name)
    {
        var parent = Directory.GetParent(EntryPath);
        if (parent == null)
        {
            return;
        }

        Directory.Move(EntryPath, System.IO.Path.Join(parent.ToString(), name));
    }
}