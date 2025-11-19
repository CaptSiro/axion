using System.Windows.Controls;

namespace axion.Views.Modals.WizardSteps;

public partial class EmptyStep : UserControl, IWizardStep
{
    public EmptyStep()
    {
        InitializeComponent();
    }


    public string Title => "";
    public bool HasNext => false;
    public IWizardStep? Next => null;

    public bool Finish() => true;
}