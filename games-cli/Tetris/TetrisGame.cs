using System;
using System.Drawing;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get => isFocused; set => isFocused = value; }
        bool isFocused = true;

        public override int DelayBetweenFrames => playGround.DelayBetweenFrames;
        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        /// Информация справа
        RightInfo rf;
        /// Игровое поле, в котором падают блоки
        TetrisPlayGround playGround;
        /// Меню паузы
        SelectionMenu pausedMenu;
        /// Выбор действия после переполнения игрового поля
        SelectionMenu gameOverMenu;
        Border border;
        int leftBorderPlayground;
        int rightBorderPlayground;
        bool isPaused = false;

        public TetrisGame(Size fieldSize, Padding p) : base(fieldSize, p)
        {
            int playGroundWidth = ConfigStorage.Current.TetrisPlayGroundWidth.Value;
            this.leftBorderPlayground = padding.Left + (fieldSize.Width / 2) - (playGroundWidth / 2);
            this.rightBorderPlayground = fieldSize.Width - (padding.Right + (fieldSize.Width / 2) - (playGroundWidth / 2));

            Init();
        }

        void Init()
        {
            playGround = new TetrisPlayGround(leftBorderPlayground, rightBorderPlayground, FieldSize, padding);

            this.rf = new RightInfo(rightBorderPlayground, playGround.NextTetromino, padding);
            this.pausedMenu = new SelectionMenu(new string[]{
                "Resume",
                "Exit"
            }, FieldSize, 0, padding);
            this.gameOverMenu = new SelectionMenu(new string[]{
                "Restart",
                "Exit"
            }, FieldSize, 0, padding);
        }

        /*
        Обработка нажатых клавиш
        */
        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                if (!playGround.GameOver)
                {
                    // Не показывать меню паузы когда
                    // игра закончилась
                    isPaused = !isPaused;
                }
            }
            if (isPaused)
            {
                pausedMenu.HandleKey(key);
            }
            if (playGround.GameOver)
            {
                gameOverMenu.HandleKey(key);
            }
            playGround.HandleKey(key);
        }

        public override void NextFrame(Drawer d)
        {
            if (isPaused)
            {
                d.Create(pausedMenu);
                CheckPauseMenuSomethingSelected(d);
                return;
            }
            else
            {
                d.Remove(pausedMenu);
            }

            if (playGround.GameOver)
            {
                playGround.IsFocused = false;
                d.Create(gameOverMenu);
                UserSelectionOnPlaygroundFilled(d);
                return;
            }

            this.border = d.CreateBorder(ConfigStorage.Current.TetrisBorderChar.Value, new Padding(
                leftBorderPlayground,
                FieldSize.Width - rightBorderPlayground,
                padding.Top,
                padding.Bottom));

            playGround.NextFrame();
            rf.ChangeNextTetromino(playGround.NextTetromino);
            rf.SetScore(playGround.Score);

            d.Create(rf);
            d.Create(playGround);
            playGroundDrawn = playGround.ElementContent;
        }

        /**
        Проверить не выбрал ли пользователь действие в меню паузы и
        выполнить это действие
        */
        void CheckPauseMenuSomethingSelected(Drawer d)
        {
            if (!pausedMenu.IsSelected)
            {
                return;
            }

            if (pausedMenu.SelectedIndex == 0)
            {
                isPaused = false;
                pausedMenu.Reuse();
            }
            else if (pausedMenu.SelectedIndex == 1)
            {
                RemoveGame(d);
                isGameOver = true;
            }
        }

        /**
        После переполнения игрового поля будет вызваться этот
        метод до тех пор пока игрок не выберет что сделать.
        */
        void UserSelectionOnPlaygroundFilled(Drawer d)
        {
            if (gameOverMenu.IsSelected)
            {
                // Игрок выбрал что делать
                RemoveGame(d);

                if (gameOverMenu.SelectedIndex == 0)
                {
                    // Перезапустить игру
                    Init();
                }
                else if (gameOverMenu.SelectedIndex == 1)
                {
                    // Выйти из игры
                    this.isGameOver = true;
                }
            }
        }

        /// Стереть, нарисованные объекты
        void RemoveGame(Drawer d)
        {
            d.Remove(playGround);
            d.Remove(rf);
            d.Remove(pausedMenu);
            d.Remove(gameOverMenu);
            d.Remove(border);
        }

        /**
        Между кадрами из другого потока может поменяться
        состояние игрового поля, поэтому сохраняется
        фактически нарисованное поле, чтобы потом именно его
        стереть, а не поле измененное из другого потока.
        */
        IDrawable[] playGroundDrawn = new IDrawable[0];
        public override void PrepareForNextFrame(Drawer d)
        {
            d.Remove(playGroundDrawn);
            d.Remove(rf);
            if (border != null)
                d.Remove(border);
        }
    }
}