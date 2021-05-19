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

            } while (!display.Exited);

            Console.CursorVisible = true;
        }
    }
}
