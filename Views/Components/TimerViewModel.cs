using System.IO;

namespace axion.Views.Components;

public class TimerViewModel : ViewModel, IEntry
{
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



    public TimerViewModel()
    {
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
}