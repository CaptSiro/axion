using axion.Utils;

namespace axion.Views.Modals.WizardSteps.DirectoryCreateStep;

public class DirectoryCreateStepViewModel : ViewModel
{
    private string _name = "";

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
}