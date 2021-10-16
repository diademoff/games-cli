using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Поле, в котором появляются и падают блоки
    */
    class TetrisPlayGround : IDrawableElement, IInteractive
    {
        public Tetromino NextTetromino => nextTetromino;
        /// Заполнено ли поле
        public bool GameOver = false;
        /// Набранные очки
        public int Score => score;
        int score = 0;
        /// Координата левого края
        int leftBorder;
        /// Координата правого края
        int rightBorder;
        /**
        Блок, который падает в данный момент
        */
        int bottomBorder;
        Tetromino fallingTetromino;
        /**
        Следующий блок после текущего
        */
        Tetromino nextTetromino;
        /**
        Упавшие блоки
        */
        List<IDrawable> tetrominoFallen = new List<IDrawable>();
        Random rnd = new Random();
        public IDrawable[] ElementContent => getContent();
        public bool IsFocused { get => isFocused; set => isFocused = value; }
        bool isFocused = true;
        Size fieldSize;
        Padding padding;
        /**
        Какая задержка должна быть между кадрами. 100 мс = 60 FPS
        */
        public int DelayBetweenFrames => 100;
        /**
        Если нажата кнопка для ускорения, то следующий кадр
        будет отрисован быстрей
        */
        bool speedUp = false;
        /**
        Когда пользователь передвигает падающий блок он перестает падать на некоторое
        время. Это свойство хранит время когда было сделано последнее перемещение блока
        */
        DateTime lastAction;
        /**
        Время в миллисекундах которое падающий блок заморожен после
        перемещения/поворота пользователем.
        */
        int freezeTime = 200;
        /**
        Время когда было сделано последнее перемещение вниз
        падающего блока
        */
        DateTime lastFall;
        /**
        Время в миллисекундах за которое падающий блок перемещается вниз
        */
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

        /**
        Сместить упавшие блоки, которые находятся выше
        удаленного ряда вниз
        */
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

        /**
        Удалить упавшие блоки с заданным Y
        */
        void RemoveRow(int y)
        {
            tetrominoFallen.RemoveAll((IDrawable x) =>
            {
                return x.Location.Y == y;
            });
        }

        /**
        Получить массив с координатами заполненных рядов
        */
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

        /**
        Находится ли какой-нибудь упавший блок на
        заданной точке
        */
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

        /**
        Перед сдвигом или поворотом блока будет выполнена проверка
        не нарушает ли это действие правила и сохранено время
        когда этот сдвиг был сделан.
        */
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

        /// Сгенерировать новый тетромино
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

        private IDrawable[] getContent()
        {
            List<IDrawable> r = new List<IDrawable>();
            r.AddRange(fallingTetromino.ElementContent);
            r.AddRange(tetrominoFallen);
            return r.ToArray();
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

        /**
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
                if (i.X <= leftBorder || i.X >= rightBorder - 1)
                {
                    return true;
                }
            }
            return false;
        }

        /**
        Пересекается ли текущий блок с упавшими
        */
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