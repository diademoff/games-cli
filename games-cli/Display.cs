/*
Управление содержимым экрана.
*/
using System;

namespace Games
{
    public class Display : IInteractive
    {
        /*
        Высчитать задержку между кадрами исходя из текущего прогресса и
        ускорения.
        */
        public int FrameDelay => game.DelayBetweenFrames;
        public bool IsGameOver => game.IsGameOver;
        int FIELD_SIZE_WIDTH;
        int FIELD_SIZE_HEIGHT;
        int INIT_DELAY = 100;

        Game game;

        /*
        Для отрисовки используется класс Drawer. Он предоставляет
        возможность отрисовать IDrawable.
        */
        Drawer drawer;
        /*
        Прогресс игрока
        */
        SnakeProgress progress;
        Random rnd = new Random();

        Padding p = new Padding(1, 1, 3, 5);
        public Display()
        {
            FIELD_SIZE_WIDTH = Console.WindowWidth;
            FIELD_SIZE_HEIGHT = Console.WindowHeight;

            drawer = new Drawer(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT);
            progress = new SnakeProgress(INIT_DELAY, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 1);

            Console.Title = "snake-cli";
            Console.CursorVisible = false;


            game = new SnakeGame(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);
            drawer.CreateBorder('·', p);

            drawer.RedrawAll();
        }

        public void NextFrame()
        {
            if (game.IsGameOver)
                return;

            /*
            Создать запросы на отрисовку
            */
            game.PrepareForNextFrame(drawer);
            game.NextFrame(drawer);

            if (game.IsGameOver)
            {
                var game_over = new MessageBox("GAME OVER", 50, 7,
                                        FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);
                drawer.Create(game_over);
            }

            /*
            Удовлетворить запросы на отрисовку
            */
            drawer.DrawToConsole();
        }

        public bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public void HandleKey(ConsoleKey key)
        {
            game.HandleKey(key);
        }
    }
}