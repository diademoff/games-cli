using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Games
{
    /// <summary>
    /// Screen management
    /// </summary>
    public class Display
    {
        /// <summary>
        /// Пользователь вышел из приложения
        /// </summary>
        public bool Exited { get; private set; } = false;
        Size fieldSize;
        /// <summary>
        /// Для отрисовки используется класс Drawer. Он предоставляет
        /// возможность отрисовать IDrawable.
        /// </summary>
        Drawer drawer;
        Screen currentScreen;
        /// <summary>
        /// Отступы от краев консоли
        /// </summary>
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
            fieldSize = new Size(Console.WindowWidth, Console.WindowHeight);
            drawer = new Drawer(fieldSize.Width, fieldSize.Height);
        }

        /// <summary>
        /// Нарисовать окно с выбором игры и ждать пока пользователь выберет
        /// </summary>
        public void StartScreen()
        {
            currentScreen = new SelectGameScreen(new string[]{
                "Snake game",     // 0
                "Snake together", // 1
                "Tetris",         // 2
                "Flappy bird",    // 3
                "Settings",       // 4
                "Exit"            // 5
            }, fieldSize, p);

            ScreenCaller.Call(currentScreen, drawer, 60).OnExit((i) =>
            {
                int selectedIndex = (int)i;

                if (selectedIndex == 0)
                    GameScreen(new SnakeGame(fieldSize, p));
                else if (selectedIndex == 1)
                    GameScreen(new SnakeTogetherGame(fieldSize, p));
                else if (selectedIndex == 2)
                    GameScreen(new TetrisGame(fieldSize, p));
                else if (selectedIndex == 3)
                    GameScreen(new FlappyBirdGame(fieldSize, p));
                else if (selectedIndex == 4)
                    ConfigurationScreen();
                else if (selectedIndex == 5)
                    Exit();
            });
        }

        void Exit()
        {
            Console.CursorVisible = true;
            Exited = true;
        }

        void GameScreen(Game game)
        {
            currentScreen = new GameScreen(game, fieldSize);

            ScreenCaller.Call(currentScreen, drawer, () => game.DelayBetweenFrames).OnExit((o) =>
            {
                StartScreen();
            });
        }

        void ConfigurationScreen()
        {
            currentScreen = new ConfigurationScreen(fieldSize, p);

            ScreenCaller.Call(currentScreen, drawer, 60).OnExit((newConfig) =>
            {
                ConfigStorage.Current = (ConfigStorage)newConfig;
                StartScreen();
            });
        }

        /// <summary>
        /// Запустить поток, который читает нажатые клавиши
        /// </summary>
        void InitKeyReading()
        {
            Thread keyReading = new Thread(ReadKeysThread);
            keyReading.IsBackground = true;
            keyReading.Start();
        }

        /// <summary>
        /// Бесконечный цикл, который читает нажатые клавиши
        /// </summary>
        void ReadKeysThread()
        {
            while (true)
            {
                ConsoleKey keyPressed = Console.ReadKey(true).Key;
                currentScreen.HandleKey(keyPressed);
            }
        }

        /// <summary>
        /// Вызывает функцию при изменении размера консоли.
        /// </summary>
        async void WindowSizeChangedHandle()
        {
            Size currentFieldSize = fieldSize;

            while (true)
            {
                int w = Console.WindowWidth;
                int h = Console.WindowHeight;

                if (w != currentFieldSize.Width || h != currentFieldSize.Height)
                {
                    if (currentScreen != null)
                        currentScreen.OnWindowSizeChanged(fieldSize);
                    currentFieldSize = fieldSize;
                }

                await Task.Delay(100);
            }
        }
    }
}