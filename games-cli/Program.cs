using System;
using System.Threading;

namespace Games
{
    class Program
    {
        static Display display;
        static void Main(string[] args)
        {
            display = new Display();

            display.SelectGame();

            do
            {
                display.NextFrame();

                Thread.Sleep(display.FrameDelay);

                if (display.IsGameOver)
                    display.SelectGame();
            } while (!display.Exited);

            Console.CursorVisible = true;
        }
    }
}
