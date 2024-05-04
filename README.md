
![Screenshot of the Candle Timer main window, showing a countdown in progress.](https://raw.githubusercontent.com/bear-evans/interval-timer/main/images/screenshot.jpg)

# Candle Timer

A Maui app for reminders at repeated intervals.

## Features

* Configurable start and end times by hour and minute of day.
* Interval is configurable by hours, minutes, and seconds.
* Audible chimes and visual countdown displayed as both numbers and a progress bar.

## Installation

The easiest way to use this application is to run it through an IDE.

1. Ensure the [.NET SDK](https://dotnet.microsoft.com/en-us/download) is installed on your device.
1. Clone this repository.
2. Open the solution using Visual Studio.
3. Build and run the application.

## Usage

Once loaded, set the start and end times of the time period you want to track, such as the start and end times of your workday, your exercise time, or the open and close times of the stock market.

Set the interval period. This is the time between chimes. An interval of three minutes starting at 9 AM, for example, will chime at 9:03 AM, 9:06 AM, 9:09 AM, and onward.

Once configured, start the timer.

If outside the set times, the timer will wait until the start time and begin chiming at intervals. It will display `Waiting...` in this situation.

The timer can be paused, during which it will not chime. If unpaused, it will begin chiming again at the set intervals. Pausing it will not cause the chimes to be delayed.

While running, the progress bar and timer text will show the time left in the current interval.

## License
Copyright 2024 Bear Evans

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.