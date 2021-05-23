using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Информация справа в игре Тетрис
    */
    class RightInfo : IDrawableElement
    {
        public IDrawable[] ElementContent => getContent();

        /**
        Отображать следующий блок, который будет создан
        после того как упадет текущий.
        */
        Tetromino nextTetromino;
        /// Место в котором нарисовать информацию о следующем блоке
        Point nextTetrominoLocation;
        public RightInfo(int rightBorder, Tetromino nextTetromino, Padding p)
        {
            this.nextTetrominoLocation = new Point(rightBorder + 3, p.Top + 3);
            this.nextTetromino = nextTetromino;
        }

        public void ChangeNextTetromino(Tetromino t)
        {
            this.nextTetromino = t;
        }

        IDrawable[] getContent()
        {
            List<IDrawable> r = new List<IDrawable>();

            for (int i = 0; i < nextTetromino.structure.Length; i++)
            {
                char c = nextTetromino.ElementContent[i].Char;
                Point location = new Point(nextTetromino.structure[i].X + nextTetrominoLocation.X,
                                    nextTetromino.structure[i].Y + nextTetrominoLocation.Y);

                r.Add(new DrawableChar(c, location));
            }

            return r.ToArray();
        }
    }
}