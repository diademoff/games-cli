using System;

namespace snake_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "snake-cli";
            Console.CursorVisible = false;
            Snake snake = new Snake('*');
            Drawer drawer = new Drawer(100, 30);
            drawer.CreateBorder('.');

            while (true)
            {
                ConsoleKey keyPressed = Console.ReadKey(true).Key;
                // ConsoleKey keyPressed = ConsoleKey.D;
                var dir = GetDirectionFromKey(keyPressed);
                if (dir == Direction.None)
                {
                    continue;
                }
                drawer.RemoveSnake(snake); // стереть старую змейку

                snake.Direction = dir;
                snake.Move();

                drawer.CreateSnake(snake);
                drawer.DrawAllToConsole();
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
