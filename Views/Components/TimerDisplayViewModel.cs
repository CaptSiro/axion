using System.Windows.Threading;

namespace axion.Views.Components;

public class TimerDisplayViewModel : ViewModel
{
    public TimerViewModel TimerViewModel { get; }
    private readonly TimeSpan _elapsed;
    private readonly EventHandler _tick;
    private readonly DispatcherTimer _dispatcher;

    public string ElapsedTime =>
        (_elapsed + TimeSpan.FromSeconds(Duration)).ToString(@"mm\:ss") + " - " + TimerViewModel.EntryName;

    public int Duration { get; private set; }

    public bool IsRunning { get; private set; }



    public TimerDisplayViewModel(TimerViewModel timer, DispatcherTimer dispatcher)
    {
        Duration = 0;
        TimerViewModel = timer;
        _elapsed = timer.Elapsed;
        _dispatcher = dispatcher;

        _tick = (_, _) =>
        {
            Duration++;
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