using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using axion.Utils;
using axion.Views.Components;
using axion.Views.Modals;

namespace axion.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly DispatcherTimer _timer;
    private MainWindowViewModel ViewModel { get; }


    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel();
        DataContext = ViewModel;

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _timer.Start();
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

    private void ListBoxItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is ListBoxItem { DataContext: DirectoryViewModel dir })
        {
            ViewModel.Load(dir);
        }

        if (sender is ListBoxItem { DataContext: TimerViewModel timer })
        {
            ViewModel.AddTimerDisplay(new TimerDisplayViewModel(timer, _timer));
        }
    }
}