using axion.Utils;

namespace axion.Views.Modals.WizardSteps.TimerCreateStep;

public class TimerCreateStepViewModel : ViewModel
{
    private string _name = "";

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
}