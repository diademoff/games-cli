using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Тетромино - это блок, который падает в игре "Тетрис"
    */
    public abstract class Tetromino : IDrawableElement
    {
        public Point[] structure => structureRotate[CurrentRotationState];
        protected int offsetX, offsetY = 0;
        protected char Symbol = '*';
        /**
        Текущий поворот. Определяется массивом из списка
        structureRotate
        */
        protected int CurrentRotationState = 0;
        /**
        Каждый массив содержит информацию о расположении блоков из
        которых состоит элемент. Каждый следующий массив в списке описывает
        положение элемента повернутого на 90 градусов относительного положения,
        описанного в предыдущем массиве.


        Координаты левого верхнего угла: (0, 0)
        Координаты блока, который на 1 ниже левого верхнего угла: (0, 1)
        Координаты блока, который на 1 правее левого верхнего угла: (1, 0)
        и т.д.
        */
        protected abstract List<Point[]> structureRotate { get; }
        public IDrawable[] ElementContent
        {
            get
            {
                List<IDrawable> r = new List<IDrawable>(4);
                foreach (var s in structureRotate[CurrentRotationState])
                {
                    r.Add(new DrawableChar(Symbol, new Point(s.X + offsetX, s.Y + offsetY)));
                }
                return r.ToArray();
            }
        }
        public void MoveDown()
        {
            this.offsetY += 1;
        }

        public Point[] TryMoveDown()
        {
            return Try(0, 1);
        }

        public void MoveLeft()
        {
            this.offsetX -= 1;
        }

        public Point[] TryMoveLeft()
        {
            return Try(-1, 0);
        }

        public void MoveRight()
        {
            this.offsetX += 1;
        }

        public Point[] TryMoveRight()
        {
            return Try(1, 0);
        }

        /**
        Сдвинуть блок на X вправо и на Y вниз. Текущий
        объект не изменится.

        Используется для проверки можно ли сдвинуть блок в каком-либо
        направлении. Своего рода гипотетическое действие, которое не
        изменяет текущий элемент.
        */
        private Point[] Try(int plusX, int plusY)
        {
            Point[] r = new Point[structure.Length];
            Point[] curr = structureRotate[CurrentRotationState];

            for (int i = 0; i < structure.Length; i++)
            {
                r[i] = new Point(curr[i].X + offsetX + plusX, curr[i].Y + offsetY + plusY);
            }

            return r;
        }

        public void Rotate()
        {
            CurrentRotationState = GetNextRotateIndex();
        }

        private int GetNextRotateIndex()
        {
            if (CurrentRotationState == structureRotate.Count - 1)
            {
                return 0;
            }
            else
            {
                return CurrentRotationState + 1;
            }
        }

        public Point[] TryRotate()
        {
            Point[] s =  structureRotate[GetNextRotateIndex()];
            Point[] withOffsets = new Point[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                withOffsets[i] = new Point(s[i].X + offsetX, s[i].Y + offsetY);
            }

            return withOffsets;
        }

        /**
        Сбросить все смещения, поместить элемент в левый
        верхний угол
        */
        public void ResetOffsets()
        {
            offsetX = offsetY = 0;
        }

        public Tetromino(int fieldWidth, int initYAxisLocation)
        {
            this.offsetX = fieldWidth / 2;
            this.offsetY = initYAxisLocation;
        }
    }
}