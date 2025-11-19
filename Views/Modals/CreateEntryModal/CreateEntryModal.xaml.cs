using System.Windows;

namespace axion.Views.Modals.CreateEntryModal;

public partial class CreateEntryModal : Window
{
    private CreateEntryModalViewModel ViewModel { get; }

    public CreateEntryModal(IWizardStep step)
    {
        InitializeComponent();
        ViewModel = new CreateEntryModalViewModel(step);
        DataContext = ViewModel;
    }
}