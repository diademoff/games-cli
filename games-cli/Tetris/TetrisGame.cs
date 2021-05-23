using System;
using System.Drawing;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public override int DelayBetweenFrames => delay();

        int delay()
        {
            if (speedUp)
            {
                speedUp = false;
                return 50;
            }
            return 200;
        }

        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        /// Информация справа
        RightInfo rf;
        /// Игровое поле, в котором падают блоки
        TetrisPlayGround playGround;
        /// Сообщение о том что игра преостановлена
        MessageBox paused_msgbx;
        /// Выбор действия после переполнения игрового поля
        SelectionMenu game_over_menu;
        Border border;
        int left_border_playground;
        int right_border_playground;
        bool isPaused = false;
        /**
        Если нажата кнопка для ускорения, то следующий кадр
        будет отрисован быстрей
        */
        bool speedUp = false;

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
            this.paused_msgbx = new MessageBox("Press ESC to resume",
                25, 4, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, padding);
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
                isPaused = !isPaused;
            }
            if (key == ConsoleKey.Spacebar ||
                key == ConsoleKey.S ||
                key == ConsoleKey.DownArrow)
            {
                speedUp = true;
            }
            if (!isPaused)
            {
                playGround.HandleKey(key);
            }
            if (playGround.GameOver)
            {
                game_over_menu.HandleKey(key);
            }
        }

        public override void NextFrame(Drawer d)
        {
            if (isPaused)
            {
                d.Create(paused_msgbx);
                return;
            }
            else
            {
                d.Remove(paused_msgbx);
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

            d.Create(rf);
            d.Create(playGround);
            playGroundDrawn = playGround.ElementContent;
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
                d.Remove(playGround);
                d.Remove(rf);
                d.Remove(game_over_menu);
                d.Remove(border);

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