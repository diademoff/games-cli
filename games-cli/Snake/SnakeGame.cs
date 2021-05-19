using System;
using System.Drawing;

namespace Games
{
    public struct SnakeBlock : IDrawable
    {
        public Point Location { get; private set; }
        public char Char { get; private set; }

        public SnakeBlock(char c, Point p)
        {
            this.Location = p;
            this.Char = c;
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    public class SnakeGame : Game
    {
        public override bool IsFocused { get => isFocused; set => isFocused = value; }

        public override int DelayBetweenFrames => frameDelay();

        /*
        Игра заканчивается когда змейка столкнулась с чем-либо и пользователь
        отказался перезапускать игру
        */
        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;

        /*
        Столкнулась ли змейка с собой или с краем.
        */
        bool snakeDead => snake.SelfIntersect() || snake.BorderIntersect(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, padding);

        int delay = 100;

        int frameDelay()
        {
            if (speedUp)
            {
                speedUp = false;
                return delay / 4;
            }
            return delay;
        }

        bool isFocused = true;
        /*
        Яблоко, которое ест змейка
        */
        Apple apple;
        Snake snake;
        /*
        Ускорение
        */
        bool speedUp = false;
        int FIELD_SIZE_WIDTH;
        int FIELD_SIZE_HEIGHT;
        Random rnd = new Random();

        MessageBox info_paused;
        Padding padding;
        bool isPaused = false;

        /*
        Сохраняет очки, набранные змейкой и выводит ит в StatusBar
        */
        SnakeProgress progress;
        SelectionMenu gameOverAction;

        public SnakeGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p)
        {
            this.FIELD_SIZE_HEIGHT = FIELD_SIZE_HEIGHT;
            this.FIELD_SIZE_WIDTH = FIELD_SIZE_WIDTH;
            this.padding = p;

            info_paused = new MessageBox("Press ESC to resume", 30, 5,
                                        FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

            Init();
        }

        /*
        Задаёт начальное состояние змейки
        */
        void Init()
        {
            progress = new SnakeProgress(delay, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, padding.Buttom - 2);
            snake = new Snake('*', padding);
            // Пересоздать окно выбора действия чтобы сбросить предыдущий выбор
            gameOverAction = new SelectionMenu(new string[]{
                    "Restart",
                    "Exit"
                }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
            RegenerateApple();
            gameOverAction.IsFocused = false;
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            if (!isPaused)
            {
                d.Remove(info_paused);
            }

            d.Remove(snake);
            d.Remove(apple);
        }

        /*
        Следующий кадр
        */
        public override void NextFrame(Drawer d)
        {
            if (isPaused)
            {
                d.Create(info_paused);
                return;
            }

            if (snakeDead)
            {
                selectGameOverAction(d);
                return;
            }

            snake.Move();
            if (snake.IsEaten(apple))
            {
                snake.AddBlock();
                progress.AppleEaten();
                RegenerateApple();
                if (delay > 50)
                {
                    delay -= 5;
                }
            }

            d.Create(snake);
            d.Create(apple);
            d.Create(progress.StatusBar);
        }

        /*
        Окно выбора что делать после столкновения змейки с чем-либо
        */
        void selectGameOverAction(Drawer d)
        {
            if (gameOverAction.IsSelected)
            {
                // Пользователь уже выбрал что делать
                if (gameOverAction.SelectedIndex == 1)
                {
                    this.isGameOver = true;
                    d.Remove(snake);
                    d.Remove(apple);
                    d.Remove(progress.StatusBar);
                    d.Remove(gameOverAction);
                    return;
                }
                else if (gameOverAction.SelectedIndex == 0)
                {
                    d.Remove(gameOverAction);
                    Init();
                    return;
                }
            }
            d.Create(gameOverAction);
            gameOverAction.IsFocused = true;
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                if (!snakeDead)
                {
                    // Не ставить на паузу если змейка срезалась и
                    // пользователь выбирает действие
                    isPaused = !isPaused;
                }
            }
            else if (key == ConsoleKey.Spacebar)
            {
                speedUp = true;
            }

            if (snake.IsFocused)
            {
                snake.HandleKey(key);
            }

            if (gameOverAction.IsFocused)
            {
                gameOverAction.HandleKey(key);
            }
        }

        void RegenerateApple()
        {
            apple = new Apple(new AppleGen(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, snake, padding), ref rnd);
        }
    }
}