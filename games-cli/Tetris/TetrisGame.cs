using System;
using System.Drawing;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public override int DelayBetweenFrames => playGround.DelayBetweenFrames;
        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        /// Информация справа
        RightInfo rf;
        /// Игровое поле, в котором падают блоки
        TetrisPlayGround playGround;
        /// Меню паузы
        SelectionMenu paused_menu;
        /// Выбор действия после переполнения игрового поля
        SelectionMenu game_over_menu;
        Border border;
        int left_border_playground;
        int right_border_playground;
        bool isPaused = false;

        public TetrisGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p) : base(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p)
        {
            int playGroundWidth = 20;
            this.left_border_playground = padding.Left + (FIELD_SIZE_WIDTH / 2) - (playGroundWidth / 2);
            this.right_border_playground = FIELD_SIZE_WIDTH - (padding.Right + (FIELD_SIZE_WIDTH / 2) - (playGroundWidth / 2));

            Init();
        }

        void Init()
        {
            playGround = new TetrisPlayGround(left_border_playground, right_border_playground, new Size(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT), padding);

            this.rf = new RightInfo(right_border_playground, playGround.NextTetromino, padding);
            this.paused_menu = new SelectionMenu(new string[]{
                "Resume",
                "Exit"
            }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
            this.game_over_menu = new SelectionMenu(new string[]{
                "Restart",
                "Exit"
            }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
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
                paused_menu.HandleKey(key);
            }
            if (playGround.GameOver)
            {
                game_over_menu.HandleKey(key);
            }
            playGround.HandleKey(key);
        }

        public override void NextFrame(Drawer d)
        {
            if (isPaused)
            {
                d.Create(paused_menu);
                CheckPauseMenuSomethingSelected(d);
                return;
            }
            else
            {
                d.Remove(paused_menu);
            }

            if (playGround.GameOver)
            {
                playGround.IsFocused = false;
                d.Create(game_over_menu);
                UserSelectionOnPlaygroundFilled(d);
                return;
            }

            this.border = d.CreateBorder('·', new Padding(
                left_border_playground,
                FIELD_SIZE_WIDTH - right_border_playground,
                padding.Top,
                padding.Buttom));

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
            if (!paused_menu.IsSelected)
            {
                return;
            }

            if (paused_menu.SelectedIndex == 0)
            {
                isPaused = false;
                paused_menu.Reuse();
            }
            else if (paused_menu.SelectedIndex == 1)
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
            if (game_over_menu.IsSelected)
            {
                // Игрок выбрал что делать
                RemoveGame(d);

                if (game_over_menu.SelectedIndex == 0)
                {
                    // Перезапустить игру
                    Init();
                }
                else if (game_over_menu.SelectedIndex == 1)
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
            d.Remove(paused_menu);
            d.Remove(game_over_menu);
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