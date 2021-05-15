using System;
using System.Threading;

namespace snake_cli
{
    class Program
    {
        const int FIELD_SIZE_WIDTH = 100;
        const int FIELD_SIZE_HEIGHT = 30;
        static int DELAY = 100;
        const int MIN_DELAY = 40;

        static Snake snake = new Snake('*');
        static Drawer drawer = new Drawer(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT);
        static Apple apple;
        static TextField bar;
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.Title = "snake-cli";
            Console.CursorVisible = false;

            // Отступ снизу для отображения информации
            Padding p = new Padding(0, 0, 0, 5);
            bar = new TextField(new System.Drawing.Point(0, FIELD_SIZE_HEIGHT - 4), FIELD_SIZE_WIDTH);
            bar.Text = "Score: 0";

            int score = 0;

            drawer.CreateBorder('.', p);
            InitKeyReading();
            RegenerateApple(p);

            while (true)
            {
                drawer.RemoveElement(snake); // стереть старую змейку

                snake.Move();

                if (snake.IsEaten(apple))
                {
                    snake.AddBlock();
                    drawer.RemoveDrawable(apple); // удалить старое яблоко
                    RegenerateApple(p);
                    score += 1;
                    bar.Text = $"Score: {score}";
                    if (DELAY > MIN_DELAY)
                    {
                        DELAY -= 10;
                    }
                }

                drawer.CreateElement(snake); // Отрисовать новую змейки
                drawer.CreateDrawable(apple); //  Отрисовать яблоко
                drawer.CreateElement(bar); // Отрисовать бар

                drawer.DrawAllToConsole();

                if (snake.SelfIntersect() || snake.BorderIntersect(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p))
                {
                    break;
                }

                Thread.Sleep(DELAY);
            }

            MessageBox box = new MessageBox("GAME OVER", 50, 7,
                                FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

            drawer.CreateElement(box);
            drawer.DrawAllToConsole();

            Console.CursorVisible = true;
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
                if (snake.IsFocused)
                {
                    snake.HandleKey(keyPressed);
                }
            }
        }
    }
}
