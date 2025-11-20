using System.Windows;
using axion.Utils;
using axion.Views.Modals;

namespace axion.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;
    }



    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        GetProjectPath();
        ViewModel.OnLoad(this);
    }

    private void GetProjectPath()
    {
        if (Storage.Contains(Constants.KeyProjectPath))
        {
            return;
        }

        var path = DirectoryModal.SelectDirectory(this);
        if (path == null)
        {
            Application.Current.Shutdown();
            return;
        }

        Storage.Set(Constants.KeyProjectPath, path);
    }
}