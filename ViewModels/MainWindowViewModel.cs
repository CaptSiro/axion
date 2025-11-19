using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using axion.Utils;
using axion.Views;
using axion.Views.Modals.CreateEntryModal;
using axion.Views.Modals.WizardSteps.SelectEntryTypeStep;

namespace axion.ViewModels;

public class MainWindowViewModel : ViewModel
{
    #region PROPERTIES

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

    private DirectoryViewModel? _loadedDirectory;

    public DirectoryViewModel? LoadedDirectory
    {
        get => _loadedDirectory;
        private set => SetProperty(ref _loadedDirectory, value);
    }

    public string CurrentPath
    {
        get {
            var projectPath = Storage.Get<string>(Constants.KeyProjectPath);
            if (projectPath == null)
            {
                return "";
            }

            return LoadedDirectory != null
                ? Path.Join(projectPath, LoadedDirectory.EntryPath)
                : projectPath;
        }
    }

    #endregion


    #region COMMANDS

    private RelayCommand<Window>? _entryNew;
    public RelayCommand<Window> EntryNew => _entryNew ??= new RelayCommand<Window>(
        EntryNewCommand, _ => true);

    private void EntryNewCommand(Window window)
    {
        var projectPath = Storage.Get<string>(Constants.KeyProjectPath);
        if (projectPath == null)
        {
            return;
        }

        var path = LoadedDirectory != null
            ? Path.Join(projectPath, LoadedDirectory.EntryPath)
            : projectPath;

        var modal = new CreateEntryModal(new SelectEntryTypeStep(path)) { Owner = window };
        if (modal.ShowDialog() == true)
        {
            Load(CurrentPath);
        }
    }


    private RelayCommand<Window>? _entryRename;
    public RelayCommand<Window> EntryRename => _entryRename ??= new RelayCommand<Window>(
        EntryRenameCommand, _ => SelectedEntry != null);

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

        var result = MessageBox.Show(
            $"Do you want to delete {SelectedEntry.EntryName}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning
        );

        if (result == MessageBoxResult.Yes)
        {
            Entries.Remove(SelectedEntry);
        }
    }

    #endregion


    public void OnLoad(Window window)
    {
        Load(Storage.Get<string>(Constants.KeyProjectPath));
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
                EntryName = Path.GetFileName(dir),
                EntryPath = dir
            });
        }

        foreach (var file in Directory.GetFiles(path))
        {
            Entries.Add(new TimerViewModel
            {
                EntryName = Path.GetFileName(file),
                EntryPath = file
            });
        }

        LoadedDirectoryName = Path.GetFileName(path);
    }
}