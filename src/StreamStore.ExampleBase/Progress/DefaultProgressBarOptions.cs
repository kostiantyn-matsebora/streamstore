

using System;
using System.Diagnostics.CodeAnalysis;
using ShellProgressBar;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
    class DefaultProgressBarOptions : ProgressBarOptions
    {
        public DefaultProgressBarOptions(bool disableBottomPercentage = true)
        {
            ProgressBarOnBottom = true;
            DisplayTimeInRealTime = true;
            ForegroundColor = ConsoleColor.Yellow;
            BackgroundColor = ConsoleColor.DarkYellow;
            DisableBottomPercentage = disableBottomPercentage;
            ForegroundColorError = ConsoleColor.Red;
        }
    }
}
