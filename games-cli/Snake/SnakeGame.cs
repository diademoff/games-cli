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
        Столкнулась ли змейка с собой или с краем.
        */
        public override bool IsGameOver => snake.SelfIntersect() || snake.BorderIntersect(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, padding);

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
        MessageBox game_over;
        Padding padding;
        bool isPaused = false;

        /*
        Сохраняет очки, набранные змейкой и выводит ит в StatusBar
        */
        SnakeProgress progress;

        public SnakeGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p)
        {
            this.FIELD_SIZE_HEIGHT = FIELD_SIZE_HEIGHT;
            this.FIELD_SIZE_WIDTH = FIELD_SIZE_WIDTH;
            this.padding = p;

            progress = new SnakeProgress(delay, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p.Buttom - 2);
            snake = new Snake('*', p);
            RegenerateApple();

            info_paused = new MessageBox("Press ESC to resume", 30, 5,
                                        FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

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

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                isPaused = !isPaused;
            }
            else if (key == ConsoleKey.Spacebar)
            {
                speedUp = true;
            }

            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
            {
                snake.ChangeDirection(Direction.Up);
            }
            else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
            {
                snake.ChangeDirection(Direction.Left);
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
            {
                snake.ChangeDirection(Direction.Right);
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
            {
                snake.ChangeDirection(Direction.Down);
            }
        }

        void RegenerateApple()
        {
            apple = new Apple(new AppleGen(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, snake, padding), ref rnd);
        }
    }
}