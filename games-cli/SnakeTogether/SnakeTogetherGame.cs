using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    public class SnakeTogetherGame : Game
    {
        private enum CloseReason
        {
            Snake1Win,
            Snake2Win,
            Draw,
            ForceClose
        }
        public override bool IsFocused { get; set; } = true;
        public override int DelayBetweenFrames => Math.Min(snake1Progress.Delay, snake2Progress.Delay);
        public override bool IsGameOver => isGameOver;
        private bool isGameOver = false;
        Apple apple;
        Snake snake1;
        Snake snake2;
        SnakeProgress snake1Progress;
        SnakeProgress snake2Progress;
        Border border => new Border(ConfigStorage.Current.SnakeGameBorderChar.Value, FieldSize.Width, FieldSize.Height, Padding);
        Random rnd = new Random();
        bool BorderIsNotDrawn = true;
        bool FinalScreen = false;

        public SnakeTogetherGame(Size fieldSize, Padding p) : base(fieldSize, p) 
        {
            Init();
        }

        void Init()
        {
            snake1 = new Snake(ConfigStorage.Current.SnakeChar.Value, 
                                Padding, 
                                Direction.Right, 
                                new Point(Padding.Left + 1, Padding.Top + 1));

            snake2 = new Snake(ConfigStorage.Current.SnakeChar.Value,
                                Padding,
                                Direction.Left,
                                new Point(FieldSize.Width - Padding.Right - 2, FieldSize.Height - Padding.Bottom - 2));

            snake1Progress = new SnakeProgress(100, FieldSize, Padding.Bottom - 1);
            snake2Progress = new SnakeProgress(100, FieldSize, Padding.Bottom - 3);

            GameOverActionMenu = GetDefaultGameOverMenu();
            MenuPaused = GetDefaultPauseMenu();
            RegenerateApple();
            FinalScreen = false;
            BorderIsNotDrawn = true;
        }

        private void CloseGame(Drawer d, CloseReason reason)
        {
            RemoveAllGame(d);

            MessageBox mbox = new MessageBox("", 40, 5, FieldSize.Width, FieldSize.Height, Padding);

            switch (reason)
            {
                case CloseReason.Snake1Win:
                    mbox.Text = $"Первый игрок выиграл со счетом {snake1Progress.Score}!";
                    break;
                case CloseReason.Snake2Win:
                    mbox.Text = $"Второй игрок выиграл со счетом {snake2Progress.Score}!";
                    break;
                case CloseReason.Draw:
                    mbox.Text = "Ничья!";
                    break;
                case CloseReason.ForceClose:
                    d.Remove(border);
                    RemoveAllGame(d);
                    isGameOver = true;
                    return;
            }

            FinalScreen = true;
            d.Create(mbox);
            d.RedrawAll();
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            if (FinalScreen)
                return;
            RemoveAllGame(d);
        }
        
        public override void NextFrame(Drawer d)
        {
            if (BorderIsNotDrawn)
            {
                d.Create(border);
                d.RedrawAll();
                BorderIsNotDrawn = false;
            }

            if (IsGameOver || FinalScreen) 
                return;

            if (IsPaused)
            {
                d.Create(MenuPaused);
                if (MenuPaused.IsSelected)
                {
                    if (MenuPaused.SelectedIndex == 0)
                    {
                        IsPaused = false;
                        MenuPaused = GetDefaultPauseMenu();
                    }
                    else if (MenuPaused.SelectedIndex == 1)
                        CloseGame(d, CloseReason.ForceClose);
                }
                return;
            }            

            snake1.Move();
            snake2.Move();

            if (IsDraw())
            {
                CloseGame(d, CloseReason.Draw);
                return;
            }

            if (IsSnakeDead(snake1, other: snake2))
            {
                CloseGame(d, CloseReason.Snake2Win);
                return;
            }

            if (IsSnakeDead(snake2, other: snake1))
            {
                CloseGame(d, CloseReason.Snake1Win);
                return;
            }

            CheckAppleIsEatenBy(snake1, snake1Progress);
            CheckAppleIsEatenBy(snake2, snake2Progress);
            CreateAllGame(d);
        }

        void RemoveAllGame(Drawer d)
        {
            d.Remove(snake1);
            d.Remove(snake2);
            d.Remove(snake1Progress.StatusBar);
            d.Remove(snake2Progress.StatusBar);
            d.Remove(apple);
            d.Remove(MenuPaused);
        }

        void CreateAllGame(Drawer d)
        {
            d.Create(snake1);
            d.Create(snake2);
            d.Create(snake1Progress.StatusBar);
            d.Create(snake2Progress.StatusBar);
            d.Create(apple);
        }

        void CheckAppleIsEatenBy(Snake snake, SnakeProgress snakeProgress)
        {
            if (snake.Head.Location == apple.Location)
            {
                snake.AddBlock();
                snakeProgress.AppleEaten();
                RegenerateApple();
            }
        }

        /**
        Лобовое столкновение это ничья
        */
        bool IsDraw()
        {
            return snake1.Head.Location.X == snake2.Head.Location.X &&
                    snake1.Head.Location.Y == snake2.Head.Location.Y;
        }

        bool IsSnakeDead(Snake snake, Snake other)
        {
            return snake.IsDead(FieldSize, Padding) || 
                   snake.SelfIntersect() ||
                   // snake врезалась в other
                   other.Blocks.Select(x => x.Location).Contains(snake.Head.Location);
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                IsPaused = !IsPaused;
                MenuPaused.IsFocused = IsPaused;
                return;
            }

            if (IsPaused)
            {
                MenuPaused.HandleKey(key);
                return;
            }

            if (FinalScreen)
            {
                if (key == ConsoleKey.Enter)
                    Init();
                else
                    return;
            }

            if (new ConsoleKey[] { ConsoleKey.W, ConsoleKey.A, ConsoleKey.S, ConsoleKey.D }.Contains(key))
                snake1.HandleKey(key);
            else if (new ConsoleKey[] { ConsoleKey.UpArrow, ConsoleKey.LeftArrow, ConsoleKey.DownArrow, ConsoleKey.RightArrow }.Contains(key))
                snake2.HandleKey(key);
        }

        void RegenerateApple()
        {
            IEnumerable<Point> avoidLocations = snake1.Blocks.Select(x => x.Location)
                                                             .Concat(snake2.Blocks.Select(x => x.Location));
            apple = new Apple(new AppleGen(FieldSize, avoidLocations, Padding), ref rnd);
        }
    }
}
