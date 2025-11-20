using System.Windows.Controls;
using axion.Views.Modals;

namespace axion.Views.WizardSteps;

public partial class SelectEntryTypeStep : UserControl, IWizardStep
{
    private SelectEntryTypeStepViewModel ViewModel { get; }
    private string Path { get; }

    public SelectEntryTypeStep(string path)
    {
        Path = path;
        InitializeComponent();
        DataContext = ViewModel = new SelectEntryTypeStepViewModel();
    }


    public string Title => "Type selection";
    public bool HasNext => true;
    public IWizardStep Next =>
        ViewModel.SelectedType switch
        {
            EntryType.Directory => new DirectoryCreateStep(Path),
            EntryType.Timer => new TimerCreateStep(Path),
            _ => throw new ArgumentOutOfRangeException()
        };

    public bool Finish() => true;
}