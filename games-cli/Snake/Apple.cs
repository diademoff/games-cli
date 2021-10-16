using System;
using System.Drawing;
using System.Linq;

namespace Games
{
    /**
    Яблоко, которое змейка должна съесть
    */
    public class Apple : IDrawable
    {
        public Point Location { get; private set; }
        public char Char => ConfigStorage.Current.SnakeGameAppleChar.Value;

        /// Создать яблоко в поле с случайным расположением
        public Apple(AppleGen applegen, ref Random rnd)
        {
            this.Location = applegen.GetRandomPoint(ref rnd);
        }
    }

    /// Apple generator
    public struct AppleGen
    {
        public int Field_width;
        public int Field_height;
        /**
        Не генерировать в заданных точках (на теле змейки)
        */
        private Point[] Avoid;
        /// Учитывать отступы
        public Padding Padding;

        public AppleGen(int field_width, int field_height, Snake snake)
        {
            Field_width = field_width;
            Field_height = field_height;
            Padding = new Padding(0, 0, 0, 0);

            Avoid = new Point[snake.Blocks.Count];

            for (int i = 0; i < snake.Blocks.Count; i++)
            {
                Avoid[i] = snake.Blocks[i].Location; // не генерировать на змейке
            }
        }

        public AppleGen(int field_width, int field_height, Snake snake, Padding p) : this(field_width, field_height, snake)
        {
            this.Padding = p;
        }

        /**
        Сгенерировать место в соответствии с заданными параметрами.
        */
        public Point GetRandomPoint(ref Random rnd)
        {
            Point point;

            do
            {
                int x = rnd.Next(Padding.Left + 1, this.Field_width - Padding.Right - 1);
                int y = rnd.Next(Padding.Top + 1, this.Field_height - Padding.Bottom - 1);
                point = new Point(x, y);
            } while (Avoid.ToList().Contains(point));

            return point;
        }
    }
}