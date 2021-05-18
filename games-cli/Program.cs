using System;
using System.Threading;

namespace Games
{
    class Program
    {
        static bool isPaused = false;
        static Display display;
        static void Main(string[] args)
        {
            display = new Display();
            InitKeyReading();

            while (true)
            {
                if (isPaused)
                {
                    display.Paused();
                    Thread.Sleep(100);
                    continue;
                }

                if (display.IsSnakeIntersect)
                {
                    break;
                }

                display.MoveSnake();

                Thread.Sleep(display.FrameDelay);
            }

            /*
            Выход из цикла означает что игра окончена.
            */
            display.GameOver();
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
                if (keyPressed == ConsoleKey.Escape)
                {
                    isPaused = !isPaused;
                    continue;
                }
                display.HandleKey(keyPressed);
            }
        }
    }
}
