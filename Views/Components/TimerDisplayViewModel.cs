using System.Windows.Threading;

namespace axion.Views.Components;

public class TimerDisplayViewModel : ViewModel
{
    public TimerViewModel TimerViewModel { get; }
    private TimeSpan _elapsed;
    private EventHandler _tick;
    private DispatcherTimer _dispatcher;

    public string ElapsedTime => TimerViewModel.EntryName + " - " + _elapsed.ToString(@"mm\:ss");
    public TimeSpan Elapsed => _elapsed;
    public bool IsRunning { get; private set; }



    public TimerDisplayViewModel(TimerViewModel timer, DispatcherTimer dispatcher)
    {
        TimerViewModel = timer;
        _elapsed = timer.Elapsed;
        _dispatcher = dispatcher;

        _tick = (s, e) =>
        {
            _elapsed = _elapsed.Add(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(ElapsedTime));
        };

        IsRunning = false;
    }



    public void Play()
    {
        _dispatcher.Tick += _tick;
        IsRunning = true;
    }

    public void Pause()
    {
        _dispatcher.Tick -= _tick;
        IsRunning = false;
    }

    public void Toggle()
    {
        if (IsRunning)
        {
            Pause();
            return;
        }

        Play();
    }
}