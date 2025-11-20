using System.Windows.Controls;
using axion.Views.Modals;

namespace axion.Views.WizardSteps;

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