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
            InitKeyReading();

            display.SelectGame();

            do
            {
                display.NextFrame();

                Thread.Sleep(display.FrameDelay);

            } while (!display.Exited);

            Console.CursorVisible = true;
        }

        /*
        Запустить поток, который читает нажатые клавиши
        */
        static void InitKeyReading()
        {
            Thread keyReading = new Thread(ReadKeysThread);
            keyReading.IsBackground = true;
            keyReading.Start();
        }

        /*
        Бесконечный цикл, который читает нажатые клавиши
        */
        static void ReadKeysThread()
        {
            while (true)
            {
                ConsoleKey keyPressed = Console.ReadKey(true).Key;
                display.HandleKey(keyPressed);
            }
        }
    }
}
