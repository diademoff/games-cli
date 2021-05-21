using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public override int DelayBetweenFrames => 200;

        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        RightInfo rf;
        TetrisPlayGround playGround;
        int left_border_playground;
        int right_border_playground;

        public TetrisGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p) : base(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p)
        {
            int playGroundWidth = 20;
            int left_border = padding.Left + (FIELD_SIZE_WIDTH / 2) - (playGroundWidth / 2);
            int right_border = FIELD_SIZE_WIDTH - (padding.Right + (FIELD_SIZE_WIDTH / 2) - (playGroundWidth / 2));
            playGround = new TetrisPlayGround(left_border, right_border, new Size(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT), padding);

            this.left_border_playground = left_border;
            this.right_border_playground = right_border;

            this.rf = new RightInfo(right_border, playGround.NextTetromino, padding);
        }

        public override void HandleKey(ConsoleKey key)
        {
            playGround.HandleKey(key);
        }

        public override void NextFrame(Drawer d)
        {
            d.CreateBorder('·', new Padding(
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
        }
    }
}