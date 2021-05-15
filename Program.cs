using System;
using System.Threading;

namespace snake_cli
{
    class Program
    {
        static int FIELD_SIZE_WIDTH;
        static int FIELD_SIZE_HEIGHT;
        static int DELAY = 100;

        static Snake snake;
        /*
        Для отрисовки используется класс Drawer. Он предоставляет
        возможность отрисовать IDrawable.
        */
        static Drawer drawer;
        static Apple apple;
        /*
        Прогресс игрока
        */
        static Progress progress;
        static Random rnd = new Random();

        static bool isPaused = false;
        static bool speedUp = false;
        static void Main(string[] args)
        {
            FIELD_SIZE_WIDTH = Console.WindowWidth;
            FIELD_SIZE_HEIGHT = Console.WindowHeight;

            drawer = new Drawer(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT);
            progress = new Progress(DELAY, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 1);

            Console.Title = "snake-cli";
            Console.CursorVisible = false;

            // Отступ снизу для отображения информации
            Padding p = new Padding(1, 1, 3, 5);

            snake = new Snake('*', p);
            drawer.CreateBorder('·', p);
            InitKeyReading();
            RegenerateApple(p);

            MessageBox info_paused = new MessageBox("Press ESC to resume", 30, 5,
                                                    FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);
            drawer.RedrawAll();
            while (true)
            {
                if (isPaused)
                {
                    drawer.Create(info_paused);
                    drawer.DrawToConsole();

                    Thread.Sleep(100);
                    continue;
                }

                drawer.Remove(info_paused);
                drawer.Remove(snake); // стереть старую змейку

                snake.Move();

                if (snake.IsEaten(apple))
                {
                    snake.AddBlock();
                    drawer.Remove(apple); // удалить старое яблоко
                    RegenerateApple(p);
                    progress.AppleEaten();
                }

                /*
                Добавить в очередь запросы на отрисовку компонентов
                */
                drawer.Create(snake);
                drawer.Create(apple);
                drawer.Create(progress.StatusBar);

                /*
                Отрисовать очередь.
                */
                drawer.DrawToConsole();

                if (snake.SelfIntersect() || snake.BorderIntersect(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p))
                {
                    break;
                }

                if (speedUp)
                {
                    Thread.Sleep(progress.Delay / 4);
                    speedUp = false;
                    continue;
                }
                Thread.Sleep(progress.Delay);
            }

            /*
            Выход из цикла означает что игра окончена.
            */
            MessageBox box = new MessageBox("GAME OVER", 50, 7,
                                FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

            drawer.Create(box);
            drawer.DrawToConsole();
            drawer.RedrawAll();

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
        изменяет направление змейки (запущен в другом потоке)
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
                if(keyPressed == ConsoleKey.Spacebar)
                {
                    speedUp = true;
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
