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


    #region ENTRIES

    public ObservableCollection<IEntry> Entries { get; } = [];
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
        private set => SetProperty(ref _loadedDirectoryName, value);
    }

    private RelayCommand<Window>? _entryRename;

    public RelayCommand<Window> EntryRename
    {
        get { return _entryRename ??= new RelayCommand<Window>(EntryRenameCommand, _ => SelectedEntry != null); }
    }

    private void EntryRenameCommand(Window window)
    {
        if (SelectedEntry == null)
        {
            return;
        }

        var name = RenameModal.Rename(SelectedEntry.EntryName, window);
        SelectedEntry.Rename(name);
    }

    private RelayCommand? _entryDelete;

    public RelayCommand EntryDelete
    {
        get { return _entryDelete ??= new RelayCommand(EntryDeleteCommand, _ => SelectedEntry != null); }
    }

    private void EntryDeleteCommand(object? o)
    {
        if (SelectedEntry == null)
        {
            return;
        }

        // todo
        //  ask for deletion
        Entries.Remove(SelectedEntry);
    }

    #endregion


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
            Entries.Add(new TimerViewModel
            {
                EntryName = file,
                EntryPath = Path.Join(path, file)
            });
        }

        LoadedDirectoryName = Path.GetFileName(path);
    }
}