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
        Size fieldSize;
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
            fieldSize = new Size(Console.WindowWidth, Console.WindowHeight);
            drawer = new Drawer(fieldSize.Width, fieldSize.Height);
        }

        /**
        Нарисовать окно с выбором игры и ждать пока пользователь выберет
        */
        public void StartScreen()
        {
            currentScreen = new SelectGameScreen(new string[]{
                "Snake game",  // 0
                "Tetris",      // 1
                "Flappy bird", // 2
                "Settings",    // 3
                "Exit"         // 4
            }, fieldSize, p);

            ScreenCaller.Call(currentScreen, drawer, 60).OnExit((i) =>
            {
                int selectedIndex = (int)i;

                if (selectedIndex == 0)
                    GameScreen(new SnakeGame(fieldSize, p));
                else if (selectedIndex == 1)
                    GameScreen(new TetrisGame(fieldSize, p));
                else if (selectedIndex == 2)
                    GameScreen(new FlappyBirdGame(fieldSize, p));
                else if (selectedIndex == 3)
                    ConfigurationScreen();
                else if (selectedIndex == 4)
                    Exited = true;
            });
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

        /**
        Запустить поток который читает нажатые клавиши
        */
        void InitKeyReading()
        {
            Thread keyReading = new Thread(ReadKeysThread);
            keyReading.IsBackground = true;
            keyReading.Start();
        }

        /**
        Бесконечный цикл который читает нажатые клавиши
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