using System;

namespace geocells
{
    sealed class ChangeConsoleColor : IDisposable
    {
        private ConsoleColor _background;
        private ConsoleColor _foreground;

        ChangeConsoleColor() 
        {
            _background = Console.BackgroundColor;
            _foreground = Console.ForegroundColor;
        }

        public void Dispose()
        {
            Console.BackgroundColor = _background;
            Console.ForegroundColor = _foreground;
        }

        public static ChangeConsoleColor To(ConsoleColor foreground, ConsoleColor background)
        {
            var restoreOriginal = new ChangeConsoleColor();
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            return restoreOriginal;
        }
    }
}
