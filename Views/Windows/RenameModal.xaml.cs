using System.Windows;
using axion.Views.Windows;

namespace axion.Views.Modals;

public partial class RenameModal
{
    public static string Rename(string value, Window window)
    {
        var modal = new RenameModal(value) { Owner = window };
        return modal.ShowDialog() == true
            ? modal.ViewModel.Value
            : value;
    }



    private RenameModalViewModel ViewModel { get; }

    public RenameModal(string value)
    {
        InitializeComponent();
        ViewModel = new RenameModalViewModel(value);
        DataContext = ViewModel;
    }
}