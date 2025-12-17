using System.IO;
using System.Windows;
using System.Windows.Controls;
using axion.Utils;
using axion.Views.Components;
using axion.Views.Modals;

namespace axion.Views.WizardSteps;

public partial class TimerCreateStep : UserControl, IWizardStep
{
    private string Path { get; }
    private TimerCreateStepViewModel ViewModel { get; }

    public TimerCreateStep(string path)
    {
        Path = path;
        InitializeComponent();
        DataContext = ViewModel = new TimerCreateStepViewModel();
    }


    public string Title => "Create timer";
    public bool HasNext => false;
    public IWizardStep? Next => null;

    public bool Finish()
    {
        if (!FileSystem.IsValidFileName(ViewModel.Name))
        {
            MessageBox.Show(
                $"Name '{ViewModel.Name}' is not valid timer name",
                "Confirm Delete",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );

            return false;
        }

        File.WriteAllText(
            System.IO.Path.Join(Path, ViewModel.Name + "." + TimerViewModel.FileExtension),
            "");

        return true;
    }
}