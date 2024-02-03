using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;

namespace CandleTimer
{
    public partial class MainPage : ContentPage
    {
        #region Variables

        // -------------------------------------------------------------
        /// <summary> Reference to the timer view model. </summary>
        private TimerViewModel timerViewModel;
        // -------------------------------------------------------------

        #endregion Variables

        // ==============================================================

        #region Constructor

        /// <summary> Main Page constructor. </summary>
        public MainPage()
        {
            // Initialize component and VM
            InitializeComponent();
            timerViewModel = new TimerViewModel();

            // countdown bindings
            BellSFX.SetBinding(MediaElement.SourceProperty, "BellDingFile");
            CountDownText.SetBinding(Label.TextProperty, "CountdownDisplay", BindingMode.TwoWay);
            TimeInputField.SetBinding(Entry.TextProperty, "TimeInput");

            // input bindings
            HoursInput.SetBinding(Entry.TextProperty, "HoursInterval", BindingMode.TwoWay);
            MinutesInput.SetBinding(Entry.TextProperty, "MinutesInterval", BindingMode.TwoWay);
            SecondsInput.SetBinding(Entry.TextProperty, "SecondsInterval", BindingMode.TwoWay);

            BindingContext = timerViewModel;

            // Set up event listeners
            timerViewModel.ChimeEvent += Chime;
            timerViewModel.CountdownChangedEvent += OnCountdownTextChanged;
            timerViewModel.TimerBarProgressEvent += OnTimeProgress;
            timerViewModel.BarColorChangedEvent += OnTimeBarColorChange;
            timerViewModel.StartTimeChangedEvent += OnStartTimeChanged;
            timerViewModel.IntervalChangedEvent += OnIntervalChanged;

            // Populate Initial Display Values
            // TO DO: This is a clumsy way to do it, rework
            OnStartTimeChanged(timerViewModel.StartTime);
            StartTimePicker.Time = timerViewModel.StartTime;

            OnIntervalChanged(timerViewModel.Interval);
            HoursInput.Text = timerViewModel.Interval.Hours.ToString();
            MinutesInput.Text = timerViewModel.Interval.Minutes.ToString();
            SecondsInput.Text = timerViewModel.Interval.Seconds.ToString();

            OnEndTimeChanged(timerViewModel.EndTime);
            EndTimePicker.Time = timerViewModel.EndTime;

        }

        #endregion Constructor

        // ==============================================================

        #region Event Listeners

        /// <summary>
        /// Handles the click event of the start button, parsing the input and starting the
        /// countdown if successful.
        /// </summary>
        /// <param name="sender"> The button that started the function. </param>
        /// <param name="args"> Arguments passed by the sender. </param>
        private void Start(Object sender, EventArgs args)
        {
            timerViewModel.IsUserPaused = false;
            timerViewModel.StartTimer();
        }

        /// <summary> Handles the click event for the reset button, canceling the timer. </summary>
        /// <param name="sender"> The button that started the function. </param>
        /// <param name="args"> Events passed by the sender. </param>
        private void Pause(Object sender, EventArgs args)
        {
            timerViewModel.IsUserPaused = true;
        }

        /// <summary> Handles the click event for the Set Interval button. </summary>
        /// <param name="sender"> The button that started the function. </param>
        /// <param name="args"> Events passed by the sender. </param>
        private void SetInterval(Object sender, EventArgs args)
        {
            int hours;
            int minutes;
            int seconds;

            if (!int.TryParse(HoursInput.Text, out hours)) { return; }
            if (!int.TryParse(MinutesInput.Text, out minutes)) { return; }
            if (!int.TryParse(SecondsInput.Text, out seconds)) { return; }
            TimeSpan newInterval = new TimeSpan(hours, minutes, seconds);

            timerViewModel.Interval = newInterval;
        }

        /// <summary> Sets the start time from user input. </summary>
        /// <param name="sender"> The time picker triggering the event. </param>
        /// <param name="args"> Information about the property change. </param>
        private void SetStartTime(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (StartTimePicker == null || timerViewModel == null) return;
            timerViewModel.StartTime = StartTimePicker.Time;
        }

        /// <summary> Sets the end time from user input. </summary>
        /// <param name="sender"> The time picker triggering the event. </param>
        /// <param name="args"> Information about the property change. </param>
        private void SetEndTime(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (StartTimePicker == null || timerViewModel == null) return;
            timerViewModel.EndTime = EndTimePicker.Time;
        }

        #endregion Event Listeners

        // ==============================================================

        #region Settings Display

        /// <summary> Updates the settings display when the start time changes. </summary>
        /// <param name="startTime"> The new start time. </param>
        public void OnStartTimeChanged(TimeSpan startTime)
        {
            StartTimeFeedback.Text = @$"Start Time = ${startTime}";
        }

        /// <summary>
        /// Updates the settings display when the end time changes.
        /// </summary>
        /// <param name="endTime">The new end time.</param>
        public void OnEndTimeChanged(TimeSpan endTime) 
        {
            EndTimeFeedback.Text = @$"End Time = ${endTime}";
        }

        /// <summary> Updates the settings display when the interval changes. </summary>
        /// <param name="interval"> The new interval. </param>
        public void OnIntervalChanged(TimeSpan interval)
        {
            IntervalDisplay.Text = interval.ToString();
        }

        #endregion Settings Display

        // ==============================================================

        #region Timer Display

        /// <summary>
        /// Callback that updates the countdown text when triggered by a property change.
        /// </summary>
        /// <param name="text"> The new text to display. </param>
        public void OnCountdownTextChanged(string text)
        {
            CountDownText.Text = text;
        }

        /// <summary> Updates the candle bar when the timer's progress changes. </summary>
        /// <param name="progress"> The new progress value. </param>
        public void OnTimeProgress(float progress)
        {
            CandleBar.ProgressTo(progress, 1, Easing.Linear);
        }

        /// <summary> Updates the progress bar's color as the timer progresses. </summary>
        /// <param name="newColor">The new color of the time bar. </param>
        public void OnTimeBarColorChange(Color newColor)
        {
            CandleBar.ProgressColor = newColor;
        }

        #endregion Timer Display

        // ==============================================================

        #region Audio

        /// <summary> Plays the timer's chime from a media source. </summary>
        private void Chime()
        {
            BellSFX.Stop();
            BellSFX.Play();
        }

        #endregion Audio
    }
}