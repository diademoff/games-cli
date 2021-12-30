using System;
using System.Drawing;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get; set; } = true;
        public override int DelayBetweenFrames => playGround.DelayBetweenFrames;
        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        RightInfo rf;
        TetrisPlayGround playGround;
        Border border;
        int leftBorderPlayground;
        int rightBorderPlayground;

        public TetrisGame(Size fieldSize, Padding p) : base(fieldSize, p)
        {
            int playGroundWidth = ConfigStorage.Current.TetrisPlayGroundWidth.Value;
            this.leftBorderPlayground = Padding.Left + (fieldSize.Width / 2) - (playGroundWidth / 2);
            this.rightBorderPlayground = fieldSize.Width - (Padding.Right + (fieldSize.Width / 2) - (playGroundWidth / 2));

            Init();
        }

        void Init()
        {
            playGround = new TetrisPlayGround(leftBorderPlayground, rightBorderPlayground, FieldSize, Padding);

            this.rf = new RightInfo(rightBorderPlayground, playGround.NextTetromino, Padding);
            this.MenuPaused = GetDefaultPauseMenu();
            this.GameOverActionMenu = GetDefaultGameOverMenu();
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
                    IsPaused = !IsPaused;
                }
            }
            if (IsPaused)
            {
                MenuPaused.HandleKey(key);
            }
            if (playGround.GameOver)
            {
                GameOverActionMenu.HandleKey(key);
            }
            playGround.HandleKey(key);
        }

        public override void NextFrame(Drawer d)
        {
            if (IsPaused)
            {
                d.Create(MenuPaused);
                CheckPauseMenuSomethingSelected(d);
                return;
            }
            else
            {
                d.Remove(MenuPaused);
            }

            if (playGround.GameOver)
            {
                playGround.IsFocused = false;
                d.Create(GameOverActionMenu);
                UserSelectionOnPlaygroundFilled(d);
                return;
            }

            this.border = d.CreateBorder(ConfigStorage.Current.TetrisBorderChar.Value, new Padding(
                leftBorderPlayground,
                FieldSize.Width - rightBorderPlayground,
                Padding.Top,
                Padding.Bottom));

            playGround.NextFrame();
            rf.ChangeNextTetromino(playGround.NextTetromino);
            rf.SetScore(playGround.Score);

            d.Create(rf);
            d.Create(playGround);
            playGroundDrawn = playGround.ElementContent;
        }

        /// <summary>
        /// Проверить не выбрал ли пользователь действие в меню паузы и
        /// выполнить это действие
        /// </summary>
        /// <param name="d"></param>
        void CheckPauseMenuSomethingSelected(Drawer d)
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

        /// <summary>
        /// После переполнения игрового поля будет вызваться этот
        /// метод до тех пор пока игрок не выберет что сделать.
        /// </summary>
        void UserSelectionOnPlaygroundFilled(Drawer d)
        {
            if (GameOverActionMenu.IsSelected)
            {
                // Игрок выбрал что делать
                RemoveGame(d);

                if (GameOverActionMenu.SelectedIndex == 0)
                {
                    // Перезапустить игру
                    Init();
                }
                else if (GameOverActionMenu.SelectedIndex == 1)
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
            d.Remove(MenuPaused);
            d.Remove(GameOverActionMenu);
            d.Remove(border);
        }

        /// <summary>
        /// Между кадрами из другого потока может поменяться
        /// состояние игрового поля, поэтому сохраняется
        /// фактически нарисованное поле, чтобы потом именно его
        /// стереть, а не поле измененное из другого потока.
        /// </summary>
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