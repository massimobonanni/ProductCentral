using Figgle;
using Figgle.Fonts;

namespace ProductCentral.LogisticCLI.Utilities;

/// <summary>
/// Provides utility methods for enhanced console output operations including colored text and timestamped messages.
/// </summary>
internal static class ConsoleUtility
{
    /// <summary>
    /// Writes a line to the console with the specified foreground color, then restores the original color.
    /// </summary>
    /// <param name="message">The message to write to the console. Defaults to an empty string.</param>
    /// <param name="foregroundColor">The foreground color to use for the message. Defaults to white.</param>
    public static void WriteLine(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        var currentForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.WriteLine(message);
        Console.ForegroundColor = currentForegroundColor;
    }

    /// <summary>
    /// Writes text to the console with the specified foreground color, then restores the original color.
    /// </summary>
    /// <param name="message">The message to write to the console. Defaults to an empty string.</param>
    /// <param name="foregroundColor">The foreground color to use for the message. Defaults to white.</param>
    public static void Write(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        var currentForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.Write(message);
        Console.ForegroundColor = currentForegroundColor;
    }

    /// <summary>
    /// Writes a line to the console with a timestamp prefix in the format [HH:mm:ss.fff] and the specified foreground color.
    /// </summary>
    /// <param name="message">The message to write to the console. Defaults to an empty string.</param>
    /// <param name="foregroundColor">The foreground color to use for the message. Defaults to white.</param>
    public static void WriteLineWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
    }

    /// <summary>
    /// Writes text to the console with a timestamp prefix in the format [HH:mm:ss.fff] and the specified foreground color.
    /// </summary>
    /// <param name="message">The message to write to the console. Defaults to an empty string.</param>
    /// <param name="foregroundColor">The foreground color to use for the message. Defaults to white.</param>
    public static void WriteWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
    {
        Write($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
    }

    /// <summary>
    /// Displays the application banner using ASCII art with the text "Logistic Console" in green color.
    /// </summary>
    /// <remarks>
    /// This method uses the Figgle library with the Banner font to create stylized ASCII text output.
    /// </remarks>
    public static void WriteApplicationBanner()
    {
        WriteLine();
        WriteLine(FiggleFonts.Banner.Render("Logistic Console"), ConsoleColor.Green);
        WriteLine();
    }

}
