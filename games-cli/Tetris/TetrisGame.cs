using System;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public override int DelayBetweenFrames => 200;

        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;

        Tetromino falling_tetromino;
        /*
        Фактически нарисованный тетрамино
        */
        IDrawable[] tetromino_drawn = new IDrawable[0];

        public TetrisGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p) : base(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p)
        {
            falling_tetromino = new TetrominoO(20, padding.Top + 1);
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
            {
                falling_tetromino.MoveLeft();
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
            {
                falling_tetromino.MoveRight();
            }
            else if (key == ConsoleKey.W)
            {
                falling_tetromino.Rotate();
            }
        }

        public override void NextFrame(Drawer d)
        {
            falling_tetromino.MoveDown();
            d.Create(falling_tetromino);

            tetromino_drawn = falling_tetromino.ElementContent; // сохранить, чтобы потом удалить
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            d.Remove(tetromino_drawn);
        }
    }
}