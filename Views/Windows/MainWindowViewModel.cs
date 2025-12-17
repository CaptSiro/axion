using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using axion.Models;
using axion.Utils;
using axion.Views.Components;
using axion.Views.Modals;
using axion.Views.WizardSteps;

namespace axion.Views.Windows;

public class MainWindowViewModel : ViewModel
{
    #region PROPERTIES

    public ObservableCollection<TimerDisplayViewModel> Timers { get; } = [];

    private TimerDisplayViewModel? _selectedTimer;
    public TimerDisplayViewModel? SelectedTimer
    {
        get => _selectedTimer;
        set => SetProperty(ref _selectedTimer, value);
    }


    public ObservableCollection<IEntry> Entries { get; } = [];
    private IEntry? _selectedEntry;

    public IEntry? SelectedEntry
    {
        get => _selectedEntry;
        set
        {
            if (!SetProperty(ref _selectedEntry, value))
            {
                return;
            }

            NoneVisibility = Visibility.Collapsed;
            TimerVisibility = Visibility.Collapsed;
            DirectoryVisibility = Visibility.Collapsed;

            switch (value)
            {
                case null:
                    NoneVisibility = Visibility.Visible;
                    break;

                case TimerViewModel:
                    TimerVisibility = Visibility.Visible;
                    break;

                case DirectoryViewModel:
                    DirectoryVisibility = Visibility.Visible;
                    break;
            }

            OnPropertyChanged(nameof(SelectedTimerViewModel));
            OnPropertyChanged(nameof(Sessions));
        }
    }


    public TimerViewModel? SelectedTimerViewModel => SelectedEntry as TimerViewModel;
    public ObservableCollection<SessionRecord> Sessions
        => SelectedTimerViewModel?.Sessions ?? [];


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
        private set
        {
            SetProperty(ref _loadedDirectory, value);
            OnPropertyChanged(nameof(CanGoBack));
        }
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
                ? LoadedDirectory.EntryPath
                : projectPath;
        }
    }


    private Visibility _noneVisibility = Visibility.Visible;
    public Visibility NoneVisibility
    {
        get => _noneVisibility;
        set => SetProperty(ref _noneVisibility, value);
    }


    private Visibility _directoryVisibility = Visibility.Collapsed;
    public Visibility DirectoryVisibility
    {
        get => _directoryVisibility;
        set => SetProperty(ref _directoryVisibility, value);
    }


    private Visibility _timerVisibility = Visibility.Collapsed;
    public Visibility TimerVisibility
    {
        get => _timerVisibility;
        set => SetProperty(ref _timerVisibility, value);
    }

    #endregion


    #region COMMANDS

    private RelayCommand? _back;
    public RelayCommand Back => _back ??= new RelayCommand(
        BackCommand, _ => CanGoBack);

    private void BackCommand(object? obj)
    {
        if (LoadedDirectory == null)
        {
            return;
        }

        Load(LoadedDirectory.Parent);
    }

    public bool CanGoBack => LoadedDirectory != null;


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
            ? LoadedDirectory.EntryPath
            : projectPath;

        var modal = new CreateEntryModal(new SelectEntryTypeStep(path)) { Owner = window };
        var result = modal.ShowDialog();
        if (result == true)
        {
            Reload();
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
        if (SelectedEntry.Rename(name))
        {
            Reload();
        }
    }


    private RelayCommand? _entryDelete;

    public RelayCommand EntryDelete =>
        _entryDelete ??= new RelayCommand(EntryDeleteCommand, _ => SelectedEntry != null);

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

        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        SelectedEntry.Delete();
        Entries.Remove(SelectedEntry);
    }


    private RelayCommand? _timerToggle;

    public RelayCommand TimerToggle =>
        _timerToggle ??= new RelayCommand(TimerToggleCommand, _ => SelectedTimer != null);

    private void TimerToggleCommand(object? obj)
    {
        SelectedTimer!.Toggle();
    }


    private RelayCommand? _timerRemove;

    public RelayCommand TimerRemove =>
        _timerRemove ??= new RelayCommand(TimerRemoveCommand, _ => SelectedTimer != null);

    private void TimerRemoveCommand(object? obj)
    {
        SelectedTimer!.TimerViewModel.RecordSession(SelectedTimer.Elapsed.Seconds);
        Timers.Remove(SelectedTimer!);
    }

    #endregion



    public void AddTimerDisplay(TimerDisplayViewModel timer)
    {
        if (Timers.All(x => x.TimerViewModel.Path != timer.TimerViewModel.Path))
        {
            Timers.Add(timer);
            timer.Play();
        }

        SelectedTimer = timer;
    }

    public void OnLoad(Window window)
    {
        Load(Storage.Get<string>(Constants.KeyProjectPath));
    }

    private void Load(string? path)
    {
        var projectPath = Storage.Get<string>(Constants.KeyProjectPath);
        if (path == null || projectPath == null || !Directory.Exists(path))
        {
            return;
        }

        if (Path.GetFullPath(projectPath) == Path.GetFullPath(path))
        {
            LoadedDirectory = null;
        }

        Entries.Clear();

        foreach (var dir in Directory.GetDirectories(path))
        {
            Entries.Add(new DirectoryViewModel(dir));
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

    private void Reload()
    {
        Load(CurrentPath);
    }

    public void Load(DirectoryViewModel directory)
    {
        LoadedDirectory = directory;
        Load(directory.EntryPath);
    }
}