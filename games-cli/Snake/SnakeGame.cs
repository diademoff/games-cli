using System;
using System.Drawing;
using System.Linq;

namespace Games
{
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
        public override bool IsFocused { get; set; } = true;

        public override int DelayBetweenFrames => frameDelay();

        /// <summary>
        /// Игра заканчивается когда змейка столкнулась с чем-либо и пользователь
        /// отказался перезапускать игру
        /// </summary>
        public override bool IsGameOver => isGameOver;
        /// <summary>
        /// Чтобы выйти из игры сделайте эту переменную true, чтобы сообщить
        /// классу, который вызвал эту игру о том что игра закончена.
        /// </summary>
        bool isGameOver = false;
        int delay;

        /// <summary>
        /// Высчитать задержку между кадрами исходя из текущего прогресса и
        /// ускорения.
        /// </summary>
        int frameDelay()
        {
            if (speedUp)
            {
                speedUp = false;
                var percent = ((double)ConfigStorage.Current.SnakeSpeedUp.Value) / 100.0;
                return (int)((double)delay * percent);
            }
            return delay;
        }

        Apple apple;
        Snake snake;
        /// <summary>
        /// Отрисовать следующий кадр быстрее (ускорить змейку)
        /// </summary>
        bool speedUp = false;
        Random rnd = new Random();

        /// <summary>
        /// Сохраняет очки, набранные змейкой и выводит их в StatusBar
        /// </summary>
        SnakeProgress progress;
        /// <summary>
        /// Нужно ли отрисовать границу
        /// </summary>
        bool drawBorder;
        /// <summary>
        /// Нарисованная граница, чтобы при выходе из игры удалить ее
        /// </summary>
        Border border;

        public SnakeGame(Size fieldSize, Padding p) : base(fieldSize, p)
        {
            Init();
        }

        /// <summary>
        /// Задаёт начальное состояние игры
        /// </summary>
        void Init()
        {
            progress = new SnakeProgress(delay, FieldSize, Padding.Bottom - 2);
            snake = new Snake(ConfigStorage.Current.SnakeChar.Value, Padding);
            // Пересоздать окно выбора действия чтобы сбросить предыдущий выбор
            GameOverActionMenu = GetDefaultGameOverMenu();
            MenuPaused = GetDefaultPauseMenu();
            RegenerateApple();
            GameOverActionMenu.IsFocused = false;
            delay = 100;
            drawBorder = true;
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            if (!IsPaused)
            {
                d.Remove(MenuPaused);
            }

            d.Remove(snake.ElementContent[snake.ElementContent.Length - 1]);
            d.Remove(apple);
        }

        public override void NextFrame(Drawer d)
        {
            if (drawBorder)
            {
                this.border = d.CreateBorder(ConfigStorage.Current.SnakeGameBorderChar.Value, Padding);
                drawBorder = false;
            }

            if (snake.IsDead(FieldSize, Padding))
            {
                selectGameOverAction(d);
                return;
            }

            if (IsPaused)
            {
                d.Create(MenuPaused);
                CheckPausedMenuSomethingSelected(d);
            }

            if (!IsPaused)
            {
                MoveSnake();
            }

            if (!isGameOver)
            {
                d.Create(snake);
                d.Create(apple);
                d.Create(progress.StatusBar);
            }
        }

        void CheckPausedMenuSomethingSelected(Drawer d)
        {
            if (!MenuPaused.IsSelected)
            {
                return;
            }

            if (MenuPaused.SelectedIndex == 0)
            {
                IsPaused = false;
                MenuPaused.Reuse();
            }
            else if (MenuPaused.SelectedIndex == 1)
            {
                RemoveGame(d);
                isGameOver = true;
            }
        }

        void MoveSnake()
        {
            snake.Move();
            if (snake.IsEaten(apple))
            {
                snake.AddBlock();
                progress.AppleEaten();
                RegenerateApple();

                /*
                Уменьшить интервал между кадрами, то есть
                увеличить скорость змейки. Миниматьный интервал между кадрами: 50
                */
                delay = Math.Max(50, delay - 5);
            }
        }

        /// <summary>
        /// Окно выбора действия после столкновения змейки с чем-либо
        /// </summary>
        void selectGameOverAction(Drawer d)
        {
            if (GameOverActionMenu.IsSelected)
            {
                // Пользователь уже выбрал что делать
                if (GameOverActionMenu.SelectedIndex == 1)
                {
                    ExitGame(d);
                    return;
                }
                else if (GameOverActionMenu.SelectedIndex == 0)
                {
                    RestartGame(d);
                    return;
                }
            }
            d.Create(GameOverActionMenu);
            GameOverActionMenu.IsFocused = true;
        }

        /// <summary>
        /// Стереть не нужное содержимое и перезапустить игру
        /// </summary>
        void RestartGame(Drawer d)
        {
            d.Remove(GameOverActionMenu);
            d.Remove(snake);
            Init();
        }

        /// <summary>
        /// Стереть ненужное содержимое и выйти из игры
        /// </summary>
        void ExitGame(Drawer d)
        {
            this.isGameOver = true;
            RemoveGame(d);
        }

        /// Стереть элементы игры
        void RemoveGame(Drawer d)
        {
            d.Remove(snake);
            d.Remove(apple);
            d.Remove(progress.StatusBar);
            d.Remove(GameOverActionMenu);
            d.Remove(MenuPaused);
            d.Remove(border);
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                if (!snake.IsDead(FieldSize, Padding))
                {
                    // Не ставить на паузу если змейка врезалась и
                    // пользователь выбирает действие
                    IsPaused = !IsPaused;
                }
            }
            else if (key == ConsoleKey.Spacebar)
            {
                speedUp = true;
            }
            
            snake.HandleKey(key);

            if (GameOverActionMenu.IsFocused)
            {
                GameOverActionMenu.HandleKey(key);
            }

            if (IsPaused)
            {
                MenuPaused.HandleKey(key);
            }
        }

        void RegenerateApple()
        {
            apple = new Apple(new AppleGen(FieldSize, snake.Blocks.Select(x => x.Location), Padding), ref rnd);
        }
    }
}