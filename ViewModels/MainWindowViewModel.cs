using System.Windows;
using axion.Utils;
using axion.Views;

namespace axion.ViewModels;

public class MainWindowViewModel
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
}