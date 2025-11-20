using System.Windows;

namespace axion.Views.Modals;

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