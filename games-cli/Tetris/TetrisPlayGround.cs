using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    /// <summary>
    /// Поле, в котором появляются и падают блоки
    /// </summary>
    class TetrisPlayGround : IDrawableElement, IInteractive
    {
        public Tetromino NextTetromino => nextTetromino;
        /// <summary>
        /// Заполнено ли поле
        /// </summary>
        public bool GameOver = false;
        /// Набранные очки
        public int Score => score;
        int score = 0;
        /// <summary>
        /// Координата левого края
        /// </summary>
        int leftBorder;
        /// <summary>
        /// Координата правого края
        /// </summary>
        int rightBorder;
        int bottomBorder;
        /// <summary>
        /// Блок, который падает в данный момент
        /// </summary>
        Tetromino fallingTetromino;
        /// <summary>
        /// Следующий блок после текущего
        /// </summary>
        Tetromino nextTetromino;
        /// <summary>
        /// Упавшие блоки
        /// </summary>
        List<IDrawable> tetrominoFallen = new List<IDrawable>();
        Random rnd = new Random();
        public IDrawable[] ElementContent => GetContent();
        public bool IsFocused { get => isFocused; set => isFocused = value; }
        bool isFocused = true;
        Size fieldSize;
        Padding padding;
        /// <summary>
        /// Какая задержка должна быть между кадрами. 100 мс = 60 FPS
        /// </summary>
        public int DelayBetweenFrames => 100;
        /// <summary>
        /// Если нажата кнопка для ускорения, то следующий кадр
        /// будет отрисован быстрей
        /// </summary>
        bool speedUp = false;
        /// <summary>
        /// Когда пользователь передвигает падающий блок он перестает падать на некоторое
        /// время.Это свойство хранит время когда было сделано последнее перемещение блока
        /// </summary>
        DateTime lastAction;
        /// <summary>
        /// Время в миллисекундах которое падающий блок заморожен после
        /// перемещения/поворота пользователем.
        /// </summary>
        int freezeTime = 200;
        /// <summary>
        ///  Время когда было сделано последнее перемещение вниз
        /// падающего блока
        /// </summary>
        DateTime lastFall;
        /// <summary>
        /// Время в миллисекундах за которое падающий блок перемещается вниз
        /// </summary>
        int fallingInterval => speedUp ? 50 : 200;

        public TetrisPlayGround(int leftBorder, int rightBorder, Size fieldSize, Padding p)
        {
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
            this.fieldSize = fieldSize;
            this.padding = p;
            this.bottomBorder = fieldSize.Height - padding.Bottom - 2;

            fallingTetromino = GetRandomTetromino();
            nextTetromino = GetRandomTetromino();
        }

        public void NextFrame()
        {
            if (IsOnBottom(fallingTetromino) ||
                IsIntersectsWithFallen(fallingTetromino.TryMoveDown()))
            {
                SwitchToNextTetromino();
                int[] filledRaws = GetFilledRaws();
                if (filledRaws.Length > 0)
                {
                    foreach (var rowY in filledRaws)
                    {
                        RemoveRow(rowY);
                        ShiftBlocksAfterRowRemoved(rowY);
                        this.score += 1;
                    }
                }
            }
            else
            {
                MoveFallingTetrominoDown();
            }
        }

        void MoveFallingTetrominoDown()
        {
            if ((DateTime.Now - lastAction).Milliseconds < freezeTime)
            {
                /*
                Не перемещать блок вниз если после последнего перемещение
                не прошло достаточно времени
                */
                return;
            }
            if((DateTime.Now - lastFall).Milliseconds < fallingInterval)
            {
                /*
                Падать нужно с заданым интервалом
                */
                return;
            }
            fallingTetromino.MoveDown();
            lastFall = DateTime.Now;
            speedUp = false; // сброс ускорения
        }

        /// <summary>
        /// Сместить упавшие блоки, которые находятся выше
        /// удаленного ряда вниз
        /// </summary>
        void ShiftBlocksAfterRowRemoved(int rowRemoved)
        {
            for (int i = 0; i < tetrominoFallen.Count; i++)
            {
                IDrawable symbol = tetrominoFallen[i];
                if (symbol.Location.Y < rowRemoved)
                {
                    symbol = new DrawableChar(symbol.Char,
                        new Point(symbol.Location.X, symbol.Location.Y + 1));
                    tetrominoFallen[i] = symbol;
                }
            }
        }

        /// <summary>
        /// Удалить упавшие блоки с заданным Y
        /// </summary>
        void RemoveRow(int y)
        {
            tetrominoFallen.RemoveAll((IDrawable x) =>
            {
                return x.Location.Y == y;
            });
        }

        /// <summary>
        /// Получить массив с координатами заполненных рядов
        /// </summary>
        int[] GetFilledRaws()
        {
            List<int> r = new List<int>();
            for (int i = bottomBorder; i >= padding.Top; i--)
            {
                bool lineFilled = true;
                for (int j = leftBorder + 1; j < rightBorder - 1; j++)
                {
                    Point p = new Point(j, i);
                    if (!isPointInFallen(p))
                    {
                        lineFilled = false;
                        break;
                    }
                }
                if (lineFilled)
                {
                    r.Add(i);
                }
            }
            return r.ToArray();
        }

        /// <summary>
        /// Находится ли какой-нибудь упавший блок на
        /// заданной точке
        /// </summary>
        bool isPointInFallen(Point p)
        {
            foreach (var i in tetrominoFallen)
            {
                if (p.X == i.Location.X && p.Y == i.Location.Y)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Перед сдвигом или поворотом блока будет выполнена проверка
        /// не нарушает ли это действие правила и сохранено время
        /// когда этот сдвиг был сделан.
        /// </summary>
        void MoveFallingLeft()
        {
            Point[] theoryTetromino = fallingTetromino.TryMoveLeft();
            if (AnyIntersects(theoryTetromino))
            {
                return;
            }
            fallingTetromino.MoveLeft();
            lastAction = DateTime.Now;
        }

        void MoveFallingRight()
        {
            Point[] theoryTetromino = fallingTetromino.TryMoveRight();
            if (AnyIntersects(theoryTetromino))
            {
                return;
            }
            fallingTetromino.MoveRight();
            lastAction = DateTime.Now;
        }

        void RotateFalling()
        {
            Point[] theoryTetromino = fallingTetromino.TryRotate();
            if (AnyIntersects(theoryTetromino))
            {
                return;
            }
            fallingTetromino.Rotate();
            lastAction = DateTime.Now;
        }

        /// <summary>
        /// Сгенерировать новый тетромино
        /// </summary>
        Tetromino GetRandomTetromino()
        {
            int fieldWidth = fieldSize.Width;
            int initYAxisLocation = padding.Top;

            switch (rnd.Next(1, 8))
            {
                case 1:
                    return new TetrominoI(fieldWidth, initYAxisLocation);
                case 2:
                    return new TetrominoJ(fieldWidth, initYAxisLocation);
                case 3:
                    return new TetrominoL(fieldWidth, initYAxisLocation);
                case 4:
                    return new TetrominoO(fieldWidth, initYAxisLocation);
                case 5:
                    return new TetrominoS(fieldWidth, initYAxisLocation);
                case 6:
                    return new TetrominoT(fieldWidth, initYAxisLocation);
                case 7:
                    return new TetrominoZ(fieldWidth, initYAxisLocation);
            }
            throw new Exception("Unreal exception GetRandomTetromino");
        }

        private IDrawable[] GetContent()
        {
            return fallingTetromino.ElementContent.Concat(tetrominoFallen).ToArray();
        }

        bool IsOnBottom(Tetromino t)
        {
            foreach (IDrawable i in t.ElementContent)
            {
                if (i.Location.Y >= bottomBorder)
                {
                    return true;
                }
            }
            return false;
        }

        void SwitchToNextTetromino()
        {
            AddCurrentTetrominoToFallen();
            fallingTetromino = nextTetromino;
            nextTetromino = GetRandomTetromino();
            if (AnyIntersects(fallingTetromino.TryMoveDown()))
            {
                // Если под появившимся блоком есть другие блоки
                // значит поле заполнено
                GameOver = true;
            }
        }

        void AddCurrentTetrominoToFallen()
        {
            foreach (IDrawable i in fallingTetromino.ElementContent)
            {
                tetrominoFallen.Add(i);
            }
        }

        /// <summary>
        /// Падающий блок не должен выходить за границы и
        /// пересекаться с другими блоками
        /// </summary>
        bool AnyIntersects(Point[] t)
        {
            return IsIntersectsWithBorder(t) || IsIntersectsWithFallen(t);
        }

        bool IsIntersectsWithBorder(Point[] t)
        {
            foreach (var i in t)
            {
                if (i.X <= leftBorder || i.X >= rightBorder - 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Пересекается ли текущий блок с упавшими
        /// </summary>
        bool IsIntersectsWithFallen(Point[] t)
        {
            foreach (var i in tetrominoFallen)
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
                if (i.Location.X <= leftBorder || i.Location.X >= rightBorder)
                {
                    return true;
                }
            }
            return false;
        }

        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar ||
                key == ConsoleKey.S ||
                key == ConsoleKey.DownArrow)
            {
                speedUp = true;
            }

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