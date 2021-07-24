using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Games
{
    /**
    Screen management
    */
    public class Display
    {
        /// Пользователь вышел из приложения
        public bool Exited { get; private set; } = false;
        Size windowSize;
        /**
        Для отрисовки используется класс Drawer. Он предоставляет
        возможность отрисовать IDrawable.
        */
        Drawer drawer;
        Screen currentScreen;
        /// Отступы от краев консоли
        Padding p = new Padding(1, 1, 3, 5);

        public Display()
        {
            Console.Title = "games-cli";
            Console.CursorVisible = false;

            SetWindowSize(Console.WindowWidth, Console.WindowHeight);

            InitKeyReading();
            WindowSizeChangedHandle();

            drawer.RedrawAll();
        }

        void SetWindowSize(int width, int height)
        {
            windowSize = new Size(Console.WindowWidth, Console.WindowHeight);
            drawer = new Drawer(windowSize.Width, windowSize.Height);
        }

        /**
        Нарисовать окно с выбором игры и ждать пока пользователь выберет
        */
        public void StartScreen()
        {
            currentScreen = new SelectGameScreen(new string[]{
                "Snake game",
                "Tetris",
                "Flappy bird",
                "Settings",
                "Exit"
            }, windowSize, p);

            ScreenCaller.Call(currentScreen, drawer, 60).OnExit((i) =>
            {
                int selectedIndex = (int)i;
                if (selectedIndex <= 2)
                {
                    if (selectedIndex == 0)
                        GameScreen(new SnakeGame(windowSize.Width, windowSize.Height, p));
                    else if (selectedIndex == 1)
                        GameScreen(new TetrisGame(windowSize.Width, windowSize.Height, p));
                    else if (selectedIndex == 2)
                        GameScreen(new FlappyBirdGame(windowSize.Width, windowSize.Height, p));
                }
                else
                {
                    if (selectedIndex == 3)
                        ConfigurationScreen();
                    else if (selectedIndex == 4)
                        Exited = true;
                }
            });
        }

        void GameScreen(Game game)
        {
            currentScreen = new GameScreen(game, windowSize);

            ScreenCaller.Call(currentScreen, drawer, () => game.DelayBetweenFrames).OnExit((o) =>
            {
                StartScreen();
            });
        }

        void ConfigurationScreen()
        {
            currentScreen = new ConfigurationScreen(windowSize, p);

            ScreenCaller.Call(currentScreen, drawer, 60).OnExit((newConfig) =>
            {
                var a = ConfigStorage.Current;
                ConfigStorage.Current = (ConfigStorage)newConfig;
                StartScreen();
            });
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
                currentScreen.HandleKey(keyPressed);
            }
        }

        /**
        Вызывает функцию при изменении размера консоли.
        */
        async void WindowSizeChangedHandle()
        {
            Size currentSize = windowSize;

            while (true)
            {
                int w = Console.WindowWidth;
                int h = Console.WindowHeight;

                if (w != currentSize.Width || h != currentSize.Height)
                {
                    if (currentScreen != null)
                        currentScreen.OnWindowSizeChanged(w, h);
                    currentSize = new Size(w, h);
                }

                await Task.Delay(100);
            }
        }
    }
}