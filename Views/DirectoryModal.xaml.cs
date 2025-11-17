using axion.ViewModels;

namespace axion.Views;

public partial class DirectoryModal
{
    public DirectoryModalViewModel ViewModel { get; }

    public DirectoryModal()
    {
        InitializeComponent();
        ViewModel = new DirectoryModalViewModel();
        DataContext = ViewModel;
    }
}