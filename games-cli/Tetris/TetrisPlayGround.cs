/*
Поле, в котором появляются и падают блоки
*/

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    class TetrisPlayGround : IDrawableElement, IInteractive
    {
        public Tetromino NextTetromino => next_tetromino;
        int falling_blocks_field_width = 20;
        // Координата левого края
        int left_border;
        // Координата правого края
        int right_border;
        /*
        Блок, который падает в данный момент
        */
        Tetromino falling_tetromino;
        /*
        Следующий блок после текущего
        */
        Tetromino next_tetromino;
        /*
        Упавшие блоки
        */
        List<IDrawable> tetromino_fallen = new List<IDrawable>();
        Random rnd = new Random();
        public IDrawable[] ElementContent => getContent();

        public bool IsFocused { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        Size field_size;
        Padding padding;

        public TetrisPlayGround(int left_border, int right_border, Size field_size, Padding p)
        {
            this.left_border = left_border;
            this.right_border = right_border;
            this.field_size = field_size;
            this.padding = p;

            falling_tetromino = GetRandomTetromino();
            next_tetromino = GetRandomTetromino();
        }

        public void NextFrame()
        {
            if (IsOnButtom(falling_tetromino) ||
                IsIntersectsWithFallen(falling_tetromino.TryMoveDown()))
            {
                SwitchToNextTetromino();
            }
            else
            {
                falling_tetromino.MoveDown();
            }
        }

        /*
        Перед сдвигом или поворотом блока будет выполнена проверка
        не нарушает ли это действие правила.
        */

        void MoveFallingLeft()
        {
            Point[] theory_tetromino = falling_tetromino.TryMoveLeft();
            if (AnyIntersects(theory_tetromino))
            {
                return;
            }
            falling_tetromino.MoveLeft();
        }

        void MoveFallingRight()
        {
            Point[] theory_tetromino = falling_tetromino.TryMoveRight();
            if (AnyIntersects(theory_tetromino))
            {
                return;
            }
            falling_tetromino.MoveRight();
        }

        void RotateFalling()
        {
            Point[] theory_tetromino = falling_tetromino.TryRotate();
            if (AnyIntersects(theory_tetromino))
            {
                return;
            }
            falling_tetromino.Rotate();
        }

        Tetromino GetRandomTetromino()
        {
            int field_width = field_size.Width;
            int init_y_location = 5;

            switch (rnd.Next(1, 8))
            {
                case 1:
                    return new TetrominoI(field_width, init_y_location);
                case 2:
                    return new TetrominoJ(field_width, init_y_location);
                case 3:
                    return new TetrominoL(field_width, init_y_location);
                case 4:
                    return new TetrominoO(field_width, init_y_location);
                case 5:
                    return new TetrominoS(field_width, init_y_location);
                case 6:
                    return new TetrominoT(field_width, init_y_location);
                case 7:
                    return new TetrominoZ(field_width, init_y_location);
            }
            throw new Exception("Unreal exception GetRandomTetromino");
        }

        bool drawBorder = true;
        private IDrawable[] getContent()
        {
            List<IDrawable> r = new List<IDrawable>();
            r.AddRange(falling_tetromino.ElementContent);
            r.AddRange(tetromino_fallen);
            return r.ToArray();
        }

        bool IsOnButtom(Tetromino t)
        {
            foreach (IDrawable i in t.ElementContent)
            {
                if (i.Location.Y >= field_size.Height - padding.Buttom - 2)
                {
                    return true;
                }
            }
            return false;
        }

        void SwitchToNextTetromino()
        {
            AddCurrentTetrominoToFallen();
            falling_tetromino = next_tetromino;
            next_tetromino = GetRandomTetromino();
        }

        void AddCurrentTetrominoToFallen()
        {
            foreach (IDrawable i in falling_tetromino.ElementContent)
            {
                tetromino_fallen.Add(i);
            }
        }

        /*
        Падающий блок не должен выходить за границы и
        пересекаться с другими блоками
        */
        bool AnyIntersects(Point[] t)
        {
            return IsIntersectsWithBorder(t) || IsIntersectsWithFallen(t);
        }

        bool IsIntersectsWithBorder(Point[] t)
        {
            foreach (var i in t)
            {
                if (i.X <= left_border || i.X >= right_border - 1)
                {
                    return true;
                }
            }
            return false;
        }

        /*
        Пересекается ли текущий блок с упавшими
        */
        bool IsIntersectsWithFallen(Point[] t)
        {
            foreach (var i in tetromino_fallen)
            {
                foreach (var j in t)
                {
                    if (i.Location.X == j.X &&
                        i.Location.Y == j.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool IsCollidesBorder(Tetromino t)
        {
            foreach (IDrawable i in t.ElementContent)
            {
                if (i.Location.X <= left_border || i.Location.X + 1 >= right_border)
                {
                    return true;
                }
            }
            return false;
        }

        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
            {
                MoveFallingLeft();
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
            {
                MoveFallingRight();
            }
            else if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
            {
                RotateFalling();
            }
        }
    }
}