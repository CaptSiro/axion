using System.Collections.ObjectModel;
using System.Windows;
using axion.Utils;
using EmptyStep = axion.Views.WizardSteps.EmptyStep;

namespace axion.Views.Modals;

public class CreateEntryModalViewModel : ViewModel
{
    #region PROPERTIES

    private ObservableCollection<IWizardStep> History { get; }

    private IWizardStep _currentStep;

    public IWizardStep CurrentStep
    {
        get => _currentStep;
        private set => SetProperty(ref _currentStep, value);
    }

    private string _nextButtonText = "Next";

    public string NextButtonText
    {
        get => _nextButtonText;
        private set => SetProperty(ref _nextButtonText, value);
    }

    #endregion


    #region COMMANDS

    private RelayCommand<Window>? _nextCommand;
    public RelayCommand<Window> NextCommand => _nextCommand ??= new RelayCommand<Window>(GoNext);

    private RelayCommand? _backCommand;
    public RelayCommand BackCommand => _backCommand ??= new RelayCommand(GoBack, CanGoBack);

    #endregion



    public CreateEntryModalViewModel() : this(new EmptyStep())
    {
    }

    public CreateEntryModalViewModel(IWizardStep first)
    {
        _currentStep = CurrentStep = first;
        History = [];
    }



    private void Update()
    {
        NextButtonText = !CurrentStep.HasNext
            ? "Finish"
            : "Next";
    }

    private void GoNext(Window window)
    {
        if (!CurrentStep.HasNext)
        {
            foreach (var step in History)
            {
                step.Finish();
            }

            if (!CurrentStep.Finish())
            {
                return;
            }

            window.DialogResult = true;
            window.Close();
            return;
        }

        var next = CurrentStep.Next;
        if (next == null)
        {
            return;
        }

        History.Add(CurrentStep);
        CurrentStep = next;
        Update();
    }

    private bool CanGoBack(object? o) => History.Count > 0;

    private void GoBack(object? o)
    {
        if (History.Count <= 0)
        {
            return;
        }

        var last = History[^1];
        History.RemoveAt(History.Count - 1);

        CurrentStep = last;
        Update();
    }
}