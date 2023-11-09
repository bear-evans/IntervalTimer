using CommunityToolkit.Maui.Views;

namespace CandleTimer
{
    public partial class MainPage : ContentPage
    {
        #region Variables

        // -------------------------------------------------------------
        /// <summary>
        /// Reference to the timer view model.
        /// </summary>
        public TimerViewModel timerViewModel;
        // -------------------------------------------------------------

        #endregion Variables

        // --------------------------- Time Variables ---------------------------
        private DateTime startTime;
        private DateTime endTime;

        // --------------------------- Settings ---------------------------
        private bool isStopped;
        private bool isRepeating;
        // ==============================================================

        #region Constructor

        /// <summary>
        /// Main Page constructor.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Set bindings
            BellSFX.SetBinding(MediaElement.SourceProperty, "BellDingFile");
            CountDownText.SetBinding(Label.TextProperty, "CountdownDisplay");
            TimeInputField.SetBinding(ContentProperty, "TimeInput");

            timerViewModel = new TimerViewModel();

            BindingContext = timerViewModel;
            Chime();
        }

        #endregion Constructor

        // ==============================================================

        #region Event Listeners

        /// <summary>
        /// Handles the click event of the start button, parsing the input and starting the
        /// countdown if successful.
        /// </summary>
        /// <param name="sender">The button that started the function.</param>
        /// <param name="args">Arguments passed by the sender.</param>
        private async void OnStartClicked(Object sender, EventArgs args)
        {
            float input = 0;
            if (float.TryParse(TimeInputField.Text, out input))
            {
                DoCountdown(input);
            }
            else
            {
                // the input was not a number
                isStopped = true;
            }
        }

        /// <summary>
        /// Handles the click event for the reset button, canceling the timer.
        /// </summary>
        /// <param name="sender">The button that started the function.</param>
        /// <param name="args">Events passed by the sender.</param>
        private async void OnResetClicked(Object sender, EventArgs args)
        {
            isStopped = true;
        }

        #endregion Event Listeners

        // ==============================================================

        #region Timer Display

        public void UpdateTimer()
        {
        }

        #endregion Timer Display

        // ==============================================================

        /// <summary>
        /// Runs the countdown.
        /// </summary>
        /// <param name="countTime">
        /// The amount of time the user wants the countdown configured to.
        /// </param>
        private async void DoCountdown(double countTime)
        {
            isStopped = false;

            // calculate times
            startTime = DateTime.Now;
            endTime = startTime.AddSeconds(countTime);
            double timeElapsed;

            // Reset the candle bar and text field
            CandleBar.Progress = 1f;
            CountDownText.Text = countTime.ToString();
            CandleBar.ProgressColor = Color.Parse("Green");

            // Progress the countdown
            double currentProgress = 0;

            while (DateTime.Now < endTime && !isStopped)
            {
                // MATH STUFF
                timeElapsed = countTime - (endTime - DateTime.Now).TotalSeconds;

                currentProgress = timeElapsed / countTime;
                CountDownText.Text = Math.Round(countTime - timeElapsed).ToString();

                CandleBar.Progress = 1 - currentProgress;
                await Task.Delay(100);
            }

            CandleBar.Progress = 1;
            CandleBar.ProgressColor = Color.Parse("Red");
        }

        // ==============================================================

        #region Audio

        /// <summary>
        /// Configures the media source for the bell ding on startup.
        /// </summary>
        private void Chime()
        {
            BellSFX.Stop();
            BellSFX.Play();
        }

        #endregion Audio
    }
}