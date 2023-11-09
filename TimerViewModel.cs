using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandleTimer
{
    public class TimerViewModel
    {
        #region Variable Declarations

        #region Audio

        // -------------------------------------------------------------
        /// <summary>
        /// The text to be displayed on the timer for testing binding contexts.
        /// </summary>
        public string TestText { get; } = "Binding Property Engaged!";

        /// <summary>
        /// The sound file used for the finished chime.
        /// </summary>
        public MediaSource BellDingFile { get; } = MediaSource.FromResource("Audio/CopperBellDing3.mp3");
        // -------------------------------------------------------------

        #endregion Audio

        #region Timer

        // -------------------------------------------------------------
        /// <summary>
        /// The text to be displayed on the countdown timer.
        /// </summary>
        public string CountDownDisplay { get; } = "";

        /// <summary>
        /// The contents of the user input field.
        /// </summary>
        public string TimeInput { get; set; }
        // -------------------------------------------------------------

        #endregion Timer

        #endregion Variable Declarations

        // ==============================================================

        #region Timer Functions

        public async void DoCountdown()
        {
        }

        public void PlayChime()
        {
        }

        #endregion Timer Functions

        // ==============================================================
    }
}