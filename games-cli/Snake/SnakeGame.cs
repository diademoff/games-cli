using System;
using System.Drawing;

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

        /**
        Игра заканчивается когда змейка столкнулась с чем-либо и пользователь
        отказался перезапускать игру
        */
        public override bool IsGameOver => isGameOver;
        /**
        Чтобы выйти из игры сделайте эту переменную true, чтобы сообщить
        классу, который вызвал эту игру о том что игра закончена.
        */
        bool isGameOver = false;
        int delay;

        /**
        Высчитать задержку между кадрами исходя из текущего прогресса и
        ускорения.
        */
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
        /**
        Отрисовать следующий кадр быстрее (ускорить змейку)
        */
        bool speedUp = false;
        Random rnd = new Random();
        /// Меню паузы с выбором действия
        SelectionMenu menuPaused;
        /// Пользователь поставил игру на паузу
        bool isPaused = false;

        /**
        Сохраняет очки, набранные змейкой и выводит их в StatusBar
        */
        SnakeProgress progress;
        SelectionMenu gameOverAction;
        /// Нужно ли отрисовать границу
        bool drawBorder;
        /// Нарисованная граница
        Border border;

        public SnakeGame(Size fieldSize, Padding p) : base(fieldSize, p)
        {
            Init();
        }

        /**
        Задаёт начальное состояние игры
        */
        void Init()
        {
            progress = new SnakeProgress(delay, FieldSize, padding.Bottom - 2);
            snake = new Snake(ConfigStorage.Current.SnakeChar.Value, padding);
            // Пересоздать окно выбора действия чтобы сбросить предыдущий выбор
            gameOverAction = new SelectionMenu(new string[]{
                    "Restart",
                    "Exit"
                }, FieldSize, 0, padding);
            menuPaused = new SelectionMenu(new string[]{
                "Resume",
                "Exit"
            }, FieldSize, 0, padding);
            RegenerateApple();
            gameOverAction.IsFocused = false;
            delay = 100;
            drawBorder = true;
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            if (!isPaused)
            {
                d.Remove(menuPaused);
            }

            d.Remove(snake.ElementContent[snake.ElementContent.Length - 1]);
            d.Remove(apple);
        }

        public override void NextFrame(Drawer d)
        {
            if (drawBorder)
            {
                this.border = d.CreateBorder(ConfigStorage.Current.SnakeGameBorderChar.Value, padding);
                drawBorder = false;
            }

            if (snake.IsDead(FieldSize, padding))
            {
                selectGameOverAction(d);
                return;
            }

            if (isPaused)
            {
                d.Create(menuPaused);
                CheckPausedMenuSomethingSelected(d);
            }

            if (!isPaused)
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
            if (!menuPaused.IsSelected)
            {
                return;
            }

            if (menuPaused.SelectedIndex == 0)
            {
                isPaused = false;
                menuPaused.Reuse();
            }
            else if (menuPaused.SelectedIndex == 1)
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

        /**
        Окно выбора действия после столкновения змейки с чем-либо
        */
        void selectGameOverAction(Drawer d)
        {
            if (gameOverAction.IsSelected)
            {
                // Пользователь уже выбрал что делать
                if (gameOverAction.SelectedIndex == 1)
                {
                    ExitGame(d);
                    return;
                }
                else if (gameOverAction.SelectedIndex == 0)
                {
                    RestartGame(d);
                    return;
                }
            }
            d.Create(gameOverAction);
            gameOverAction.IsFocused = true;
        }

        /**
        Стереть не нужное содержимое и перезапустить игру
        */
        void RestartGame(Drawer d)
        {
            d.Remove(gameOverAction);
            d.Remove(snake);
            Init();
        }

        /**
        Стереть ненужное содержимое и выйти из игры
        */
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
            d.Remove(gameOverAction);
            d.Remove(menuPaused);
            d.Remove(border);
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                if (!snake.IsDead(FieldSize, padding))
                {
                    // Не ставить на паузу если змейка врезалась и
                    // пользователь выбирает действие
                    isPaused = !isPaused;
                }
            }
            else if (key == ConsoleKey.Spacebar)
            {
                speedUp = true;
            }
            
            snake.HandleKey(key);

            if (gameOverAction.IsFocused)
            {
                gameOverAction.HandleKey(key);
            }

            if (isPaused)
            {
                menuPaused.HandleKey(key);
            }
        }

        void RegenerateApple()
        {
            apple = new Apple(new AppleGen(FieldSize, snake, padding), ref rnd);
        }
    }
}