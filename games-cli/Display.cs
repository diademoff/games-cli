using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Games
{
    /*
    Управление содержимым экрана. Выбор игр.
    */
    public class Display : IInteractive
    {
        public int FrameDelay
        {
            get
            {
                if (game == null)
                {
                    return 0;
                }
                return game.DelayBetweenFrames;
            }
        }
        /// Игра закончилась, то есть игрок вышел из нее
        public bool IsGameOver => game.IsGameOver;
        /// Пользователь вышел из приложения
        public bool Exited { get; private set; } = false;
        int FIELD_SIZE_WIDTH;
        int FIELD_SIZE_HEIGHT;
        Game game;
        SelectionMenu sm;

        /**
        Для отрисовки используется класс Drawer. Он предоставляет
        возможность отрисовать IDrawable.
        */
        Drawer drawer;
        Random rnd = new Random();
        /// Список объектов, которым отправлять нажатые клавиши
        List<IInteractive> keyHandlers = new List<IInteractive>();
        /// Отступы от краев консоли
        Padding p = new Padding(1, 1, 3, 5);
        public Display()
        {
            InitKeyReading();
            WindowSizeChangedHandle((w, h) => SetWindowSize(w, h));

            SetWindowSize(Console.WindowWidth, Console.WindowHeight);

            Console.Title = "games-cli";
            Console.CursorVisible = false;
        }

        void SetWindowSize(int width, int height)
        {
            FIELD_SIZE_WIDTH = Console.WindowWidth;
            FIELD_SIZE_HEIGHT = Console.WindowHeight;

            drawer = new Drawer(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT);

            sm = new SelectionMenu(new string[]{
                "Snake game",
                "Tetris",
                "Flappy bird",
                "Exit"
            }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, defaultSelected: 0, p);

            drawer.RedrawAll();
        }

        /**
        Нарисовать окно с выбором игры и ждать пока пользователь выберет
        */
        public void SelectGame()
        {
            sm.IsSelected = false;
            do
            {
                if (!keyHandlers.Contains(sm))
                    keyHandlers.Add(sm);
                drawer.Create(sm);
                drawer.DrawToConsole();
                Thread.Sleep(100);
            } while (!sm.IsSelected);

            keyHandlers.Remove(sm);
            drawer.Remove(sm);

            if (sm.SelectedIndex == 0)
            {
                game = new SnakeGame(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);
            }
            else if (sm.SelectedIndex == 1)
            {
                game = new TetrisGame(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);
            }
            else if (sm.SelectedIndex == 2)
            {
                game = new FlappyBirdGame(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);
            }
            else if (sm.SelectedIndex == 3)
            {
                Exited = true;
            }

            keyHandlers.Add(game);
        }

        public void NextFrame()
        {
            if (game == null || Exited)
            {
                return;
            }

            /*
            Создать запросы на отрисовку
            */
            game.PrepareForNextFrame(drawer);
            game.NextFrame(drawer);

            /*
            Удовлетворить запросы на отрисовку
            */
            drawer.DrawToConsole();
        }

        public bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public void HandleKey(ConsoleKey key)
        {
            foreach (IInteractive handler in keyHandlers)
            {
                if (handler.IsFocused)
                {
                    handler.HandleKey(key);
                }
            }
        }

        /**
        Запустить поток, который читает нажатые клавиши
        */
        void InitKeyReading()
        {
            Thread keyReading = new Thread(ReadKeysThread);
            keyReading.IsBackground = true;
            keyReading.Start();
        }

        /**
        Бесконечный цикл, который читает нажатые клавиши
        */
        void ReadKeysThread()
        {
            while (true)
            {
                ConsoleKey keyPressed = Console.ReadKey(true).Key;
                this.HandleKey(keyPressed);
            }
        }

        /**
        Вызывает функцию при изменении размера консоли.
        */
        async void WindowSizeChangedHandle(Action<int, int> onWindowSizeChanged)
        {
            Size currentSize = new Size(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT);

            while (true)
            {
                int w = Console.WindowWidth;
                int h = Console.WindowHeight;

                if (w != currentSize.Width || h != currentSize.Height)
                {
                    onWindowSizeChanged.Invoke(w, h);
                    currentSize = new Size(w, h);
                }

                await Task.Delay(100);
            }
        }
    }
}