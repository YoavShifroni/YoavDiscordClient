using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace YoavDiscordClient
{
    /// <summary>
    /// A countdown timer that uses a Timer and Stopwatch for precise timing.
    /// I took inspiration from this code on stackoverflow: https://stackoverflow.com/questions/6191576/seconds-countdown-timer
    /// </summary>
    public class CountDownTimer2 : IDisposable
    {
        /// <summary>
        /// The UI timer responsible for triggering periodic updates.
        /// </summary>
        private Timer _timer = new Timer();

        /// <summary>
        /// Tracks the elapsed time since the timer started.
        /// </summary>
        private Stopwatch _stpWatch = new Stopwatch();

        /// <summary>
        /// Gets the remaining time for the countdown.
        /// </summary>
        private TimeSpan TimeLeft =>
            (_totalDuration.TotalMilliseconds - _stpWatch.ElapsedMilliseconds) > 0
            ? TimeSpan.FromMilliseconds(_totalDuration.TotalMilliseconds - _stpWatch.ElapsedMilliseconds)
            : TimeSpan.Zero;

        /// <summary>
        /// The total duration of the countdown.
        /// </summary>
        private TimeSpan _totalDuration;

        /// <summary>
        /// The interval at which the timer updates, in milliseconds.
        /// </summary>
        private int StepMs;

        /// <summary>
        /// Indicates whether the countdown has finished.
        /// </summary>
        private bool _isFinished;

        /// <summary>
        /// The remaining time formatted as a string (mm:ss.fff).
        /// </summary>
        private string TimeLeftMsStr => TimeLeft.ToString(@"mm\:ss\.fff");

        /// <summary>
        /// Initializes a new instance of this class with a specified duration.
        /// </summary>
        /// <param name="min">The countdown duration in minutes.</param>
        /// <param name="sec">The countdown duration in seconds.</param>
        public CountDownTimer2(int min, int sec)
        {
            SetTime(min, sec);
            Init();
        }

        /// <summary>
        /// Handles the timer's Tick event to update the UI and check for completion.
        /// </summary>
        /// <param name="sender">The event source.</param>
        /// <param name="e">Event data.</param>
        private void TimerTick(object sender, EventArgs e)
        {
            DiscordFormsHolder.getInstance().LoginForm.ShowCooldownTimerOnLabel(this.TimeLeftMsStr);

            if (this.TimeLeft == TimeSpan.Zero && !_isFinished)
            {
                this._isFinished = true;
                this.Stop();
                MessageBox.Show("Timer finished, you can try to login again");
                DiscordFormsHolder.getInstance().LoginForm.ToggleLoginStatus(true);
                this.Dispose();
            }
        }

        /// <summary>
        /// Initializes the timer and event handlers.
        /// </summary>
        private void Init()
        {
            StepMs = 77;
            this._timer.Tick += new EventHandler(TimerTick);
            this._isFinished = false;
        }

        /// <summary>
        /// Sets the total countdown duration.
        /// </summary>
        /// <param name="min">The duration in minutes.</param>
        /// <param name="sec">The duration in seconds.</param>
        private void SetTime(int min, int sec)
        {
            this._totalDuration = TimeSpan.FromSeconds(min * 60 + sec);
        }

        /// <summary>
        /// Starts the countdown timer.
        /// </summary>
        public void Start()
        {
            this._timer.Start();
            _stpWatch.Start();
        }

        /// <summary>
        /// Stops the countdown timer.
        /// </summary>
        private void Stop()
        {
            this._timer.Stop();
            _stpWatch.Stop();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="CountDownTimer2"/>.
        /// </summary>
        public void Dispose()
        {
            this._timer.Dispose();
        }
    }
}
