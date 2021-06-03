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
        /**
        Столкнулась ли змейка с собой или с краем.
        */
        bool snakeDead => snake.SelfIntersect() || snake.BorderIntersect(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, padding);
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
                return delay / 4;
            }
            return delay;
        }

        bool isFocused = true;
        /**
        Яблоко, которое ест змейка
        */
        Apple apple;
        Snake snake;
        /**
        Ускорение
        */
        bool speedUp = false;
        Random rnd = new Random();
        /// Меню паузы с выбором действия
        SelectionMenu menu_paused;
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

        public SnakeGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p) : base(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p)
        {
            Init();
        }

        /**
        Задаёт начальное состояние игры
        */
        void Init()
        {
            progress = new SnakeProgress(delay, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, padding.Bottom - 2);
            snake = new Snake('*', padding);
            // Пересоздать окно выбора действия чтобы сбросить предыдущий выбор
            gameOverAction = new SelectionMenu(new string[]{
                    "Restart",
                    "Exit"
                }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
            menu_paused = new SelectionMenu(new string[]{
                "Resume",
                "Exit"
            }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
            RegenerateApple();
            gameOverAction.IsFocused = false;
            delay = 100;
            drawBorder = true;
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            if (!isPaused)
            {
                d.Remove(menu_paused);
            }

            d.Remove(snake.ElementContent[snake.ElementContent.Length - 1]);
            d.Remove(apple);
        }

        /**
        Следующий кадр
        */
        public override void NextFrame(Drawer d)
        {
            if (drawBorder)
            {
                this.border = d.CreateBorder('·', padding);
                drawBorder = false;
            }

            if (snakeDead)
            {
                selectGameOverAction(d);
                return;
            }

            if (isPaused)
            {
                d.Create(menu_paused);
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
            if (!menu_paused.IsSelected)
            {
                return;
            }

            if (menu_paused.SelectedIndex == 0)
            {
                isPaused = false;
                menu_paused.Reuse();
            }
            else if (menu_paused.SelectedIndex == 1)
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
                if (delay > 50)
                {
                    delay -= 5;
                }
            }
        }

        /**
        Окно выбора что делать после столкновения змейки с чем-либо
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
            d.Remove(menu_paused);
            d.Remove(border);
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                if (!snakeDead)
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

            if (snake.IsFocused)
            {
                snake.HandleKey(key);
            }

            if (gameOverAction.IsFocused)
            {
                gameOverAction.HandleKey(key);
            }

            if (isPaused)
            {
                menu_paused.HandleKey(key);
            }
        }

        void RegenerateApple()
        {
            apple = new Apple(new AppleGen(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, snake, padding), ref rnd);
        }
    }
}