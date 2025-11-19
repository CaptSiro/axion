using System.Windows;
using axion.ViewModels.Modals;

namespace axion.Views;

public partial class DirectoryModal
{
    public static string? SelectDirectory(Window window)
    {
        var modal = new DirectoryModal { Owner = window };
        return modal.ShowDialog() == true
            ? modal.ViewModel.SelectedPath
            : null;
    }



    public DirectoryModalViewModel ViewModel { get; }

    public DirectoryModal()
    {
        InitializeComponent();
        ViewModel = new DirectoryModalViewModel();
        DataContext = ViewModel;
    }
}