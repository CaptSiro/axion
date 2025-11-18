using axion.ViewModels.Modals;

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