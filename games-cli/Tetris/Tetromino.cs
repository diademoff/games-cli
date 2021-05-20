/*
Тетромино - это блок, который падает в игре "Тетрис"
*/
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public abstract class Tetromino : IDrawableElement
    {
        protected int offset_x, offset_y = 0;
        protected char Symbol = '*';
        /*
        Текущий поворот. Определяется массивом из списка
        structure_rotate
        */
        protected int CurrentRotationState = 0;
        /*
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
        public void MoveLeft()
        {
            this.offset_x -= 1;
        }
        public void MoveRight()
        {
            this.offset_x += 1;
        }

        public void Rotate()
        {
            if (CurrentRotationState == structure_rotate.Count - 1){
                CurrentRotationState = 0;
            }
            else
            {
                CurrentRotationState += 1;
            }
        }

        public Tetromino(int field_width, int init_y_location)
        {
            this.offset_x = field_width / 2;
            this.offset_y = init_y_location;
        }
    }
}