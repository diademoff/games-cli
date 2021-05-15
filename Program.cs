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
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.Title = "snake-cli";
            Console.CursorVisible = false;
            drawer.CreateBorder('.');

            Thread keyReading = new Thread(ReadKeysThread);
            keyReading.IsBackground = false;
            keyReading.Start();

            RegenerateApple();

            while (true)
            {
                drawer.RemoveSnake(snake); // стереть старую змейку

                snake.Move();

                if (snake.IsEaten(apple))
                {
                    snake.AddBlock();
                    drawer.RemoveDrawable(apple); // удалить старое яблоко
                    RegenerateApple();
                }

                drawer.CreateSnake(snake); // Отрисовать новую змейки
                drawer.CreateDrawable(apple); //  Отрисовать яблоко

                drawer.DrawAllToConsole();

                Thread.Sleep(DELAY);
            }
        }

        static void RegenerateApple()
        {
            apple = new Apple(new AppleGen(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, snake), ref rnd);
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
                // ConsoleKey keyPressed = ConsoleKey.D;
                var dir = GetDirectionFromKey(keyPressed);
                if (dir != Direction.None)
                {
                    snake.Direction = dir;
                }
            }
        }


        static Direction GetDirectionFromKey(ConsoleKey key)
        {
            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
            {
                return Direction.Up;
            }
            else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
            {
                return Direction.Left;
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
            {
                return Direction.Right;
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
            {
                return Direction.Down;
            }

            return Direction.None;
        }
    }
}
