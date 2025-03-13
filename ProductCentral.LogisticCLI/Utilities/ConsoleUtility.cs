using Figgle;

namespace ProductCentral.LogisticCLI.Utilities
{
    internal static class ConsoleUtility
    {
        public static void WriteLine(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var currentForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(message);
            Console.ForegroundColor = currentForegroundColor;
        }

        public static void Write(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            var currentForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.Write(message);
            Console.ForegroundColor = currentForegroundColor;
        }

        public static void WriteLineWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
        }

        public static void WriteWithTimestamp(string message = "", ConsoleColor foregroundColor = ConsoleColor.White)
        {
            Write($"[{DateTime.Now:HH:mm:ss.fff}] - {message}", foregroundColor);
        }

        public static void WriteApplicationBanner()
        {
            WriteLine();
            WriteLine(FiggleFonts.Banner.Render("Logistic Console"), ConsoleColor.Green);
            WriteLine();
        }

    }
}
