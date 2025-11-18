using System.Windows;
using axion.Utils;
using axion.ViewModels;

namespace axion.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;
    }



    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!Storage.Contains(Constants.KeyPath))
        {
            MainWindowViewModel.SelectDirectoryCommand(this);
        }

        if (!Storage.Contains(Constants.KeyPath))
        {
            Application.Current.Shutdown();
        }

        ViewModel.OnLoad(this);
    }
}