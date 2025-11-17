using System.Windows;
using axion.Support;
using Microsoft.Win32;

namespace axion.ViewModels;

public class DirectoryModalViewModel : ViewModelBase
{
    private string? _selectedPath;
    public string? SelectedPath
    {
        get => _selectedPath;
        set => SetProperty(ref _selectedPath, value);
    }




    private RelayCommand? _browse;

    public RelayCommand Browse
    {
        get { return _browse ??= new RelayCommand(BrowseCommand, _ => true); }
    }

    public void BrowseCommand(object? obj)
    {
        var dialog = new OpenFolderDialog();
        var result = dialog.ShowDialog();
        if (result != true)
        {
            return;
        }

        SelectedPath = dialog.FolderName;
        Console.WriteLine(SelectedPath);
    }



    private RelayCommand<Window>? _ok;

    public RelayCommand<Window> Ok
    {
        get { return _ok ??= new RelayCommand<Window>(OkCommand, _ => SelectedPath != null); }
    }

    private static void OkCommand(Window window)
    {
        window.DialogResult = true;
        window.Close();
    }



    private RelayCommand<Window>? _cancel;

    public RelayCommand<Window> Cancel
    {
        get { return _cancel ??= new RelayCommand<Window>(CancelCommand, _ => true); }
    }

    private static void CancelCommand(Window window)
    {
        window.DialogResult = false;
        window.Close();
    }
}