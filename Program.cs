using System;
using System.Threading;

namespace snake_cli
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
                    display.Flush();
                    Thread.Sleep(100);
                    continue;
                }

                if (display.IsSnakeIntersect)
                {
                    break;
                }

                NextFrame();

                Thread.Sleep(display.FrameDelay);
            }

            /*
            Выход из цикла означает что игра окончена.
            */
            display.GameOver();
        }

        static void NextFrame()
        {
            display.UnPause(); // стереть надпись паузы
            display.RemoveSnake(); // Удалить старую змейку

            display.MoveSnake();

            if (display.IsAppleEaten)
            {
                display.AddBlock();
            }

            display.Draw(); // добавить в очередь на отрисовку всё содержимое

            display.Flush(); // отрисовать очередь
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
