using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /// <summary>
    /// Информация справа в игре Тетрис
    /// </summary>
    class RightInfo : IDrawableElement
    {
        public IDrawable[] ElementContent => GetContent();

        /// <summary>
        /// Отображать следующий блок, который будет создан
        /// после того как упадет текущий.
        /// </summary>
        Tetromino nextTetromino;
        /// <summary>
        /// Место, в котором нарисовать информацию о следующем блоке
        /// </summary>
        Point nextTetrominoLocation;
        /// <summary>
        /// Текстовое поле, в котором написано кол-во набранных очков
        /// </summary>
        TextField scoreField;
        int scoreCount = 0;
        public RightInfo(int rightBorder, Tetromino nextTetromino, Padding p)
        {
            this.nextTetrominoLocation = new Point(rightBorder + 3, p.Top + 3);
            this.nextTetromino = nextTetromino;
        }

        public void SetScore(int score)
        {
            scoreCount = score;
        }

        public void ChangeNextTetromino(Tetromino t)
        {
            this.nextTetromino = t;
        }

        IDrawable[] GetContent()
        {
            List<IDrawable> r = new List<IDrawable>();

            for (int i = 0; i < nextTetromino.structure.Length; i++)
            {
                char c = nextTetromino.ElementContent[i].Char;
                Point location = new Point(nextTetromino.structure[i].X + nextTetrominoLocation.X,
                                    nextTetromino.structure[i].Y + nextTetrominoLocation.Y);

                r.Add(new DrawableChar(c, location));
            }

            this.scoreField = new TextField(new Point(nextTetrominoLocation.X, nextTetrominoLocation.Y + 7), 8,
                                    $"Score: {this.scoreCount}");
            r.AddRange(scoreField.ElementContent);

            return r.ToArray();
        }
    }
}