using System.Windows;
using axion.Support;
using axion.Views;

namespace axion.ViewModels;

public class MainWindowViewModel
{
    private RelayCommand<Window>? _selectDirectory;

    public RelayCommand<Window> SelectDirectory
    {
        get { return _selectDirectory ??= new RelayCommand<Window>(SelectDirectoryCommand, _ => true); }
    }

    private static void SelectDirectoryCommand(Window window)
    {
        var modal = new DirectoryModal { Owner = window };
        if (modal.ShowDialog() != true)
        {
            return;
        }

        var path = modal.ViewModel.SelectedPath;
        Console.WriteLine(path);
    }
}