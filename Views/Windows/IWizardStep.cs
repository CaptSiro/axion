namespace axion.Views.Modals;

public interface IWizardStep
{
    public string Title { get; }

    public bool HasNext { get; }

    public IWizardStep? Next { get; }

    public bool Finish();
}