using axion.Utils;

namespace axion.Views.Modals.WizardSteps.SelectEntryTypeStep;

public class SelectEntryTypeStepViewModel : ViewModel
{
    private EntryType _selectedType;
    public EntryType SelectedType {
        get => _selectedType;
        set => SetProperty(ref _selectedType, value);
    }
}