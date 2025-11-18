using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using axion.Utils;
using axion.Views;

namespace axion.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public static void SelectDirectoryCommand(Window window)
    {
        var modal = new DirectoryModal { Owner = window };
        if (modal.ShowDialog() != true)
        {
            return;
        }

        var path = modal.ViewModel.SelectedPath;
        if (path == null)
        {
            return;
        }

        Storage.Set(Constants.KeyPath, path);
    }



    private RelayCommand<Window>? _selectDirectory;

    public RelayCommand<Window> SelectDirectory
    {
        get { return _selectDirectory ??= new RelayCommand<Window>(SelectDirectoryCommand, _ => true); }
    }

    public ObservableCollection<IEntry> Entries { get; set; } = [];
    private IEntry? _selectedEntry;
    public IEntry? SelectedEntry
    {
        get => _selectedEntry;
        set => SetProperty(ref _selectedEntry, value);
    }

    private string? _loadedDirectoryName;
    public string? LoadedDirectoryName
    {
        get => _loadedDirectoryName;
        set => SetProperty(ref _loadedDirectoryName, value);
    }



    public void OnLoad(Window window)
    {
        Load(Storage.Get<string>(Constants.KeyPath));
    }

    private void Load(string? path)
    {
        if (path == null || !Directory.Exists(path))
        {
            return;
        }

        Entries.Clear();

        foreach (var dir in Directory.GetDirectories(path))
        {
            Entries.Add(new DirectoryViewModel
            {
                EntryName = dir,
                EntryPath = Path.Join(path, dir)
            });
        }

        foreach (var file in Directory.GetFiles(path))
        {
            Entries.Add(new FileViewModel
            {
                EntryName = file,
                EntryPath = Path.Join(path, file)
            });
        }

        LoadedDirectoryName = Path.GetFileName(path);
    }
}