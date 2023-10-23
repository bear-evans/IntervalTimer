namespace CandleTimer
{
    public partial class MainPage : ContentPage
    {
        private DateTime startTime;
        private DateTime endTime;

        public MainPage()
        {
            InitializeComponent();
        }

        // =============================================================================

        /// <summary>
        /// Runs the countdown.
        /// </summary>
        /// <param name="countTime">
        /// The amount of time the user wants the countdown configured to.
        /// </param>
        private async void DoCountdown(double countTime)
        {
            // calculate times
            startTime = DateTime.Now;
            endTime = startTime.AddSeconds(countTime);
            double timeElapsed;

            // Reset the candle bar and text field
            CandleBar.Progress = 1f;
            CountDownText.Text = countTime.ToString();

            // Progress the countdown
            double currentProgress = 0;

            while (DateTime.Now < endTime)
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

        // =============================================================================

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
            }
        }

        /// <summary>
        /// Handles the click event for the reset button, canceling the timer.
        /// </summary>
        /// <param name="sender">The button that started the function.</param>
        /// <param name="args">Events passed by the sender.</param>
        private async void OnResetClicked(Object sender, EventArgs args)
        {
        }

        // =============================================================================
    }
}