using System.Windows;
using axion.Utils;

namespace axion.Views.Windows;

public class RenameModalViewModel : ViewModel
{
    #region PROPERTIES

    public string Value { get; private set; }

    private string _label = "";

    public string Label
    {
        get => _label;
        set => SetProperty(ref _label, value);
    }

    private string _name = "";

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    #endregion



    #region COMMANDS

    private RelayCommand<Window>? _ok;

    public RelayCommand<Window> Ok =>
        _ok ??= new RelayCommand<Window>(OkCommand, OkCanExecute);

    private bool OkCanExecute(Window window)
    {
        var name = Name.Trim();
        return name.Length != 0 && name != Value;
    }

    private void OkCommand(Window window)
    {
        Value = Name.Trim();
        window.DialogResult = true;
        window.Close();
    }



    private RelayCommand<Window>? _cancel;

    public RelayCommand<Window> Cancel =>
        _cancel ??= new RelayCommand<Window>(CancelCommand, _ => true);

    private static void CancelCommand(Window window)
    {
        window.DialogResult = false;
        window.Close();
    }

    #endregion



    public RenameModalViewModel(string value)
    {
        Value = value;
        Name = value;
    }

    public RenameModalViewModel() : this("")
    {
    }
}