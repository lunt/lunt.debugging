using System;
using Lunt.Diagnostics;

namespace Lunt.Debugging
{
    /// <summary>
    /// Writes build log messages to the console.
    /// </summary>
    public sealed class ConsoleLog : IBuildLog
    {
        /// <summary>
        /// Writes a message to the build log using the specified verbosity and log level.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        public void Write(Verbosity verbosity, LogLevel level, string message)
        {
            try
            {
                Console.ForegroundColor = GetColor(level);
                Console.WriteLine("[{0}] {1}", level.ToString().Substring(0, 1), message);
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private static ConsoleColor GetColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Information:
                    return ConsoleColor.White;
                case LogLevel.Verbose:
                    return ConsoleColor.Gray;
                case LogLevel.Debug:
                    return ConsoleColor.DarkGray;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}