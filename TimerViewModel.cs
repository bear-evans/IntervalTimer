using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Platform;
using System;

namespace CandleTimer
{
    public class TimerViewModel
    {
        #region Variable Declarations

        #region App Flags

        // -------------------------------------------------------------
        /// <summary>
        /// Operational status of the timer functions. If false, intervals are not counted and no
        /// chimes will be played.
        /// </summary>
        public bool IsRunning
        {
            get => isRunning;

            set
            {
                isRunning = value;
                IsRunningChangedEvent?.Invoke(isRunning);
            }
        }
        private bool isRunning = false;

        /// <summary> Event fired when the operational status of the timer changes. </summary>
        public Action<bool> IsRunningChangedEvent { get; set; } = delegate { };

        /// <summary>
        /// True if the user has paused the timer. False if the timer is not paused or was paused by
        /// the time frame.
        /// </summary>
        public bool IsUserPaused
        {
            get => isUserPaused;

            set
            {
                isUserPaused = value;
                UserPausedChangedEvent?.Invoke(isUserPaused);
            }
        }
        private bool isUserPaused;

        /// <summary> Event fired when the user pauses or unpauses </summary>
        public Action<bool> UserPausedChangedEvent { get; set; } = delegate { };
        // -------------------------------------------------------------

        #endregion App Flags

        #region Internal Time Variables

        // -------------------------------------------------------------

        // -------------------------------------------------------------

        #endregion Internal Time Variables

        #region Audio

        // -------------------------------------------------------------
        /// <summary> The sound file used for the finished chime. </summary>
        public MediaSource BellDingFile { get; } = MediaSource.FromResource("Audio/CopperBellDing3.mp3");

        /// <summary> Event that notifies when the timer chimes. </summary>
        public Action ChimeEvent { get; set; }
        // -------------------------------------------------------------

        #endregion Audio

        #region Timer

        // -------------------------------------------------------------
        /// <summary> The time passed since the start time. </summary>
        public TimeSpan TimeSinceStart
        {
            get => timeSinceStart;

            private set
            {
                timeSinceStart = value;
                TimeSinceStartChangedEvent?.Invoke(timeSinceStart);
            }
        }
        private TimeSpan timeSinceStart;

        /// <summary> Event invoked when the time since start has been recalculated. </summary>
        public Action<TimeSpan> TimeSinceStartChangedEvent { get; set; } = delegate { };

        /// <summary> The text to be displayed on the countdown timer. </summary>
        public string CountdownDisplay
        {
            get => countdownDisplay;

            private set
            {
                countdownDisplay = value;
                CountdownChangedEvent?.Invoke(countdownDisplay);
            }
        }
        private string countdownDisplay;

        /// <summary> Event fired when the countdown display changes. </summary>
        public Action<string> CountdownChangedEvent { get; set; } = delegate { };

        /// <summary> The color of the timer's progress bar. </summary>
        public Color TimerBarColor
        {
            get => timerBarColor;

            private set
            {
                timerBarColor = value;
                BarColorChangedEvent?.Invoke(timerBarColor);
            }
        }
        private Color timerBarColor;

        /// <summary>
        /// Event fired when the color of the progress bar changes.
        /// </summary>
        public Action<Color> BarColorChangedEvent { get; set; } = delegate { };

        /// <summary>
        /// The float representing the fill value of the progress bar. 0 is empty, 1 is full.
        /// </summary>
        public float TimerBarProgress
        {
            get { return timerBarProgress; }

            private set
            {
                timerBarProgress = value;
                TimerBarProgressEvent?.Invoke(timerBarProgress);
            }
        }
        private float timerBarProgress;

        /// <summary> Event fired when the timer progress amount changes. </summary>
        public Action<float> TimerBarProgressEvent { get; set; } = delegate { };
        // -------------------------------------------------------------

        #endregion Timer

        #region Colors

        // -------------------------------------------------------------
        private Color barColorFull = Color.FromArgb("1eeb55");
        private Color barColorHalf = Color.FromArgb("ddff00");
        private Color barColorEmpty = Color.FromArgb("f22a1f");
        private Color barColorIdle = Color.FromRgb(255, 255, 190);
        // -------------------------------------------------------------

        #endregion Colors

        #region User Input

        // -------------------------------------------------------------
        /// <summary> The user-set start time for the timer. </summary>
        public TimeSpan StartTime
        {
            get => startTime;

            set
            {
                startTime = value;
                StartTimeChangedEvent?.Invoke(startTime);
            }
        }
        private TimeSpan startTime;

        /// <summary> Event fired when StartTime changes. </summary>
        public Action<TimeSpan> StartTimeChangedEvent { get; set; } = delegate { };

        /// <summary> The user-set end time for the timer. </summary>
        public TimeSpan EndTime
        {
            get => endTime;

            set
            {
                endTime = value;
                EndTimeChangedEvent?.Invoke(endTime);
            }
        }
        private TimeSpan endTime;

        /// <summary> Event fired when EndTime changes. </summary>
        public Action<TimeSpan> EndTimeChangedEvent { get; set; } = delegate { };

        // -------------------------------------------------------------

        /// <summary> The contents of the hour input field. </summary>
        public string HoursIntervalInput { get; set; }

        /// <summary> The contents of the minutes input field. </summary>
        public string MinutesIntervalInput { get; set; }

        /// <summary> The contents of the seconds input field. </summary>
        public string SecondsIntervalInput { get; set; }

        /// <summary> The interval for the chime loop, expressed as a TimeSpan object. </summary>
        public TimeSpan Interval
        {
            get => interval;

            set
            {
                interval = value;
                IntervalChangedEvent?.Invoke(interval);
                intervalInSeconds = interval.TotalSeconds;
            }
        }
        private TimeSpan interval;
        private double intervalInSeconds;

        /// <summary> Event fired when the Interval changes. </summary>
        public Action<TimeSpan> IntervalChangedEvent { get; set; } = delegate { };

        // -------------------------------------------------------------

        /// <summary> The contents of the user input field. </summary>
        public string TimeInput { get; set; }

        /// <summary> Amount of time left before the chime. </summary>
        public double RemainingSeconds
        {
            get => remainingSeconds;

            private set
            {
                lastSeconds = remainingSeconds;
                remainingSeconds = value;
            }
        }
        private double remainingSeconds;
        private double lastSeconds;
        // -------------------------------------------------------------

        #endregion User Input

        #endregion Variable Declarations

        // ==============================================================

        #region Constructor

        public TimerViewModel()
        {
            StartTime = new TimeSpan(9, 30, 0);
            EndTime = new TimeSpan(16, 30, 0);
            Interval = new TimeSpan(0, 3, 0);
        }

        #endregion Constructor

        // ==============================================================

        #region Primary App Functions

        public void StartTimer()
        {
            RunTimer();
        }

        public async void RunTimer()
        {
            // timer is started
            IsRunning = true;

            // Only run the timer function if the user has not paused the timer.
            while (!IsUserPaused)
            {
                if (CheckRunTimes())
                {
                    // Calculate remaining time in the current interval from the start time.
                    TimeSinceStart = DateTime.Now.TimeOfDay - startTime;
                    RemainingSeconds = TimeSinceStart.TotalSeconds % interval.TotalSeconds;

                    // play chime when the counter resets (why this works with the last saved time greater I'm not sure, wierd date time stuff)
                    if (lastSeconds > RemainingSeconds)
                    {
                        PlayChime();
                    }

                    SetBarProgress((interval.TotalSeconds - RemainingSeconds) / interval.TotalSeconds);

                    CountdownDisplay = $"{FormatCountdown(TimeSpan.FromSeconds(intervalInSeconds - remainingSeconds))}";

                    await Task.Delay(TimeSpan.FromSeconds(0.25));
                } else
                {
                    DisplayWaiting();

                    await Task.Delay(TimeSpan.FromSeconds(0.5));
                }

            }

            DisplayPaused();
        }

        /// <summary> Checks if the current time of day is between the start time and end times. </summary>
        /// <returns> True if the timer was switched on. False if not. </returns>
        private bool CheckRunTimes()
        {
            if (DateTime.Now.TimeOfDay > StartTime && DateTime.Now.TimeOfDay < EndTime)
            {
                return true;
            }

            return false;
        }

        #endregion Primary App Functions

        // ==============================================================

        #region Display Functions

        private string FormatCountdown(TimeSpan timeSpan)
        {
            if (interval.Hours > 0)
            {
                return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
            else
            {
                return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
            }
        }

        /// <summary>
        /// Sets the timer bar to the paused state.
        /// </summary>
        private void SetBarPaused()
        {
            TimerBarProgress = 1f;
            TimerBarColor = barColorIdle;
        }

        /// <summary>
        /// Sets the bar's progress and associated color.
        /// </summary>
        /// <param name="progress">The current progress value.</param>
        private void SetBarProgress(double progress)
        {
            TimerBarProgress = (float)progress;

            if (progress > 0.6f) 
            {
                TimerBarColor = barColorFull;
            }
            if (progress <= 0.6f && progress > .33f)
            {
                TimerBarColor = barColorHalf;
            }
            if (progress <= .33f)
            {
                TimerBarColor = barColorEmpty;
            }
        }

        /// <summary> Invokes the chime event and any other effects to be added later. </summary>
        private void PlayChime()
        {
            ChimeEvent?.Invoke();
        }

        /// <summary>
        /// Changes the countdown bar to display a paused state.
        /// </summary>
        private void DisplayPaused()
        {
            CountdownDisplay = "Paused";
            SetBarPaused();
        }

        /// <summary>
        /// Changes the countdown bar to display a waiting state.
        /// </summary>
        private void DisplayWaiting()
        {
            CountdownDisplay = "Waiting...";
            SetBarPaused();
        }

        /// <summary> Alerts the user that the input field contains invalid input. </summary>
        private void AlertInvalidInput()
        {
        }

        #endregion Display Functions

        // ==============================================================
    }
}