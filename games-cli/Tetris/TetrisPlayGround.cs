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
        public Tetromino NextTetromino => next_tetromino;
        public bool GameOver = false;
        /// Набранные очки
        public int Score => score;
        int score = 0;
        /// Координата левого края
        int left_border;
        /// Координата правого края
        int right_border;
        /**
        Блок, который падает в данный момент
        */
        int buttom_border;
        Tetromino falling_tetromino;
        /**
        Следующий блок после текущего
        */
        Tetromino next_tetromino;
        /**
        Упавшие блоки
        */
        List<IDrawable> tetromino_fallen = new List<IDrawable>();
        Random rnd = new Random();
        public IDrawable[] ElementContent => getContent();
        public bool IsFocused { get => isFocused; set => isFocused = value; }
        bool isFocused = true;
        Size field_size;
        Padding padding;
        /// Какая задержка должна быть между кадрами
        public int DelayBetweenFrames => getDelay();
        int getDelay()
        {
            if (speedUp)
            {
                speedUp = false;
                return 50;
            }
            return 200;
        }
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

        public TetrisPlayGround(int left_border, int right_border, Size field_size, Padding p)
        {
            this.left_border = left_border;
            this.right_border = right_border;
            this.field_size = field_size;
            this.padding = p;
            this.buttom_border = field_size.Height - padding.Buttom - 2;

            falling_tetromino = GetRandomTetromino();
            next_tetromino = GetRandomTetromino();
        }

        public void NextFrame()
        {
            if (IsOnButtom(falling_tetromino) ||
                IsIntersectsWithFallen(falling_tetromino.TryMoveDown()))
            {
                SwitchToNextTetromino();
                int[] filledRaws = GetFilledRaws();
                if (filledRaws.Length > 0)
                {
                    foreach (var row_y in filledRaws)
                    {
                        RemoveRow(row_y);
                        ShiftBlocksAfterRowRemoved(row_y);
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
            falling_tetromino.MoveDown();
        }

        /**
        Сместить упавшие блоки, которые находятся выше
        удаленного ряда вниз
        */
        void ShiftBlocksAfterRowRemoved(int row_removed)
        {
            for (int i = 0; i < tetromino_fallen.Count; i++)
            {
                IDrawable symbol = tetromino_fallen[i];
                if (symbol.Location.Y < row_removed)
                {
                    symbol = new DrawableChar(symbol.Char,
                        new Point(symbol.Location.X, symbol.Location.Y + 1));
                    tetromino_fallen[i] = symbol;
                }
            }
        }

        /**
        Удалить упавшие блоки с заданным Y
        */
        void RemoveRow(int y)
        {
            tetromino_fallen.RemoveAll((IDrawable x) =>
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
            for (int i = buttom_border; i >= padding.Top; i--)
            {
                bool line_filled = true;
                for (int j = left_border + 1; j < right_border - 1; j++)
                {
                    Point p = new Point(j, i);
                    if (!isPointInFallen(p))
                    {
                        line_filled = false;
                        break;
                    }
                }
                if (line_filled)
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
            foreach (var i in tetromino_fallen)
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
            Point[] theory_tetromino = falling_tetromino.TryMoveLeft();
            if (AnyIntersects(theory_tetromino))
            {
                return;
            }
            falling_tetromino.MoveLeft();
            lastAction = DateTime.Now;
        }

        void MoveFallingRight()
        {
            Point[] theory_tetromino = falling_tetromino.TryMoveRight();
            if (AnyIntersects(theory_tetromino))
            {
                return;
            }
            falling_tetromino.MoveRight();
            lastAction = DateTime.Now;
        }

        void RotateFalling()
        {
            Point[] theory_tetromino = falling_tetromino.TryRotate();
            if (AnyIntersects(theory_tetromino))
            {
                return;
            }
            falling_tetromino.Rotate();
            lastAction = DateTime.Now;
        }

        /// Сгенерировать новый тетромино
        Tetromino GetRandomTetromino()
        {
            int field_width = field_size.Width;
            int init_y_location = padding.Top;

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
                if (i.Location.Y >= buttom_border)
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
            if (AnyIntersects(falling_tetromino.TryMoveDown()))
            {
                // Если под появившимся блоком есть другие блоки
                // значит поле заполнено
                GameOver = true;
            }
        }

        void AddCurrentTetrominoToFallen()
        {
            foreach (IDrawable i in falling_tetromino.ElementContent)
            {
                tetromino_fallen.Add(i);
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
                if (i.X <= left_border || i.X >= right_border - 1)
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
                if (i.Location.X <= left_border || i.Location.X >= right_border)
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