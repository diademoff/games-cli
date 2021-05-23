using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public override int DelayBetweenFrames => delay();

        int delay()
        {
            if(speedUp)
            {
                speedUp = false;
                return 50;
            }
            return 200;
        }

        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        RightInfo rf;
        TetrisPlayGround playGround;
        MessageBox paused_msgbx;
        SelectionMenu game_over_menu;
        Border border;
        int left_border_playground;
        int right_border_playground;
        bool isPaused = false;
        bool speedUp = false;
        int left_border;
        int right_border;

        public TetrisGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p) : base(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p)
        {
            int playGroundWidth = 20;
            this.left_border = padding.Left + (FIELD_SIZE_WIDTH / 2) - (playGroundWidth / 2);
            this.right_border = FIELD_SIZE_WIDTH - (padding.Right + (FIELD_SIZE_WIDTH / 2) - (playGroundWidth / 2));

            Init();
        }

        void Init()
        {
            playGround = new TetrisPlayGround(left_border, right_border, new Size(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT), padding);

            this.left_border_playground = left_border;
            this.right_border_playground = right_border;

            this.rf = new RightInfo(right_border, playGround.NextTetromino, padding);
            this.paused_msgbx = new MessageBox("Press ESC to resume",
                25, 4, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, padding);
            this.game_over_menu = new SelectionMenu(new string[]{
                "Restart",
                "Exit"
            }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                isPaused = !isPaused;
            }
            if(key == ConsoleKey.Spacebar ||
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

        void UserSelectionOnPlaygroundFilled(Drawer d)
        {
            if (game_over_menu.IsSelected)
            {
                d.Remove(playGround);
                d.Remove(rf);
                d.Remove(game_over_menu);
                d.Remove(border);


                if (game_over_menu.SelectedIndex == 0)
                {
                    // restart
                    Init();
                }
                else if (game_over_menu.SelectedIndex == 1)
                {
                    this.isGameOver = true;
                }
            }
        }

        /*
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
            if(border != null)
                d.Remove(border);
        }
    }
}