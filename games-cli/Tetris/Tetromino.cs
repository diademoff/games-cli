using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Тетромино - это блок, который падает в игре "Тетрис"
    */
    public abstract class Tetromino : IDrawableElement
    {
        public Point[] structure => structure_rotate[CurrentRotationState];
        protected int offset_x, offset_y = 0;
        protected char Symbol = '*';
        /**
        Текущий поворот. Определяется массивом из списка
        structure_rotate
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
        protected abstract List<Point[]> structure_rotate { get; }
        public IDrawable[] ElementContent
        {
            get
            {
                List<IDrawable> r = new List<IDrawable>(4);
                foreach (var s in structure_rotate[CurrentRotationState])
                {
                    r.Add(new DrawableChar(Symbol, new Point(s.X + offset_x, s.Y + offset_y)));
                }
                return r.ToArray();
            }
        }
        public void MoveDown()
        {
            this.offset_y += 1;
        }

        public Point[] TryMoveDown()
        {
            return Try(0, 1);
        }

        public void MoveLeft()
        {
            this.offset_x -= 1;
        }

        public Point[] TryMoveLeft()
        {
            return Try(-1, 0);
        }

        public void MoveRight()
        {
            this.offset_x += 1;
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
        private Point[] Try(int plus_x, int plus_y)
        {
            Point[] r = new Point[structure.Length];
            Point[] curr = structure_rotate[CurrentRotationState];

            for (int i = 0; i < structure.Length; i++)
            {
                r[i] = new Point(curr[i].X + offset_x + plus_x, curr[i].Y + offset_y + plus_y);
            }

            return r;
        }

        public void Rotate()
        {
            CurrentRotationState = GetNextRotateIndex();
        }

        private int GetNextRotateIndex()
        {
            if (CurrentRotationState == structure_rotate.Count - 1)
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
            Point[] s =  structure_rotate[GetNextRotateIndex()];
            Point[] with_offsets = new Point[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                with_offsets[i] = new Point(s[i].X + offset_x, s[i].Y + offset_y);
            }

            return with_offsets;
        }

        /**
        Сбросить все смещения, поместить элемент в левый
        верхний угол
        */
        public void ResetOffsets()
        {
            offset_x = offset_y = 0;
        }

        public Tetromino(int field_width, int init_y_location)
        {
            this.offset_x = field_width / 2;
            this.offset_y = init_y_location;
        }
    }
}