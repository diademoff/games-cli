using System;
using System.Threading;

namespace snake_cli
{
    class Program
    {
        const int FIELD_SIZE_WIDTH = 100;
        const int FIELD_SIZE_HEIGHT = 30;
        static int DELAY = 100;

        static Snake snake = new Snake('*');
        static Drawer drawer = new Drawer(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT);
        static Apple apple;
        static Progress progress = new Progress(DELAY, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 1);
        static Random rnd = new Random();

        static bool isPaused = false;

        static void Main(string[] args)
        {
            Console.Title = "snake-cli";
            Console.CursorVisible = false;

            // Отступ снизу для отображения информации
            Padding p = new Padding(0, 0, 0, 2);

            drawer.CreateBorder('·', p);
            InitKeyReading();
            RegenerateApple(p);

            MessageBox info = new MessageBox("Press ESC to resume", 30, 5,
                                                    FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

            while (true)
            {
                if (isPaused)
                {
                    drawer.Create(info);
                    drawer.DrawAllToConsole();

                    Thread.Sleep(100);
                    continue;
                }

                drawer.Remove(info);
                drawer.Remove(snake); // стереть старую змейку

                snake.Move();

                if (snake.IsEaten(apple))
                {
                    snake.AddBlock();
                    drawer.Remove(apple); // удалить старое яблоко
                    RegenerateApple(p);
                    progress.AppleEaten();
                }

                drawer.Create(snake); // Отрисовать новую змейки
                drawer.Create(apple); //  Отрисовать яблоко
                drawer.Create(progress.StatusBar); // Отрисовать бар

                drawer.DrawAllToConsole();

                if (snake.SelfIntersect() || snake.BorderIntersect(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p))
                {
                    break;
                }

                Thread.Sleep(progress.Delay);
            }

            MessageBox box = new MessageBox("GAME OVER", 50, 7,
                                FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

            drawer.Create(box);
            drawer.DrawAllToConsole();

            Console.CursorVisible = true;
            Console.WriteLine();
        }

        static void InitKeyReading()
        {
            Thread keyReading = new Thread(ReadKeysThread);
            keyReading.IsBackground = true;
            keyReading.Start();
        }

        static void RegenerateApple(Padding p)
        {
            apple = new Apple(new AppleGen(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, snake, p), ref rnd);
        }

        /*
        Бесконечный цикл, который читает нажатые клавиши и
        изменяет направление змейки
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
                if (snake.IsFocused)
                {
                    snake.HandleKey(keyPressed);
                }
            }
        }
    }
}
