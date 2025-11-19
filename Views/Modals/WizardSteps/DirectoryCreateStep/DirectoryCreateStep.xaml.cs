using System.IO;
using System.Windows;
using System.Windows.Controls;
using axion.Utils;

namespace axion.Views.Modals.WizardSteps.DirectoryCreateStep;

public partial class DirectoryCreateStep : UserControl, IWizardStep
{
    private string Path { get; }
    private DirectoryCreateStepViewModel ViewModel { get; }
    public DirectoryCreateStep(string path)
    {
        Path = path;
        InitializeComponent();
        DataContext = ViewModel = new DirectoryCreateStepViewModel();
    }


    public string Title => "Create Directory";
    public bool HasNext => false;
    public IWizardStep? Next => null;

    public bool Finish()
    {
        if (!FileSystem.IsValidDirectoryName(ViewModel.Name))
        {
            MessageBox.Show(
                $"Name '{ViewModel.Name}' is not valid timer name",
                "Confirm Delete",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );

            return false;
        }

        Directory.CreateDirectory(System.IO.Path.Join(Path, ViewModel.Name));
        return true;
    }
}