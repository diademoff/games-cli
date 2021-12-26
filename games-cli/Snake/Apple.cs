using System;
using System.Collections.Generic;
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
        public int FieldWidth;
        public int FieldHeight;
        /**
        Не генерировать в заданных точках (на теле змейки)
        */
        private IEnumerable<Point> Avoid;
        /// Учитывать отступы
        public Padding Padding;

        /// <summary>
        /// Генератор яблок
        /// </summary>
        /// <param name="fieldSize"></param>
        /// <param name="avoidPoints">Не генерировать на этих точках</param>
        public AppleGen(Size fieldSize, IEnumerable<Point> avoidPoints)
        {
            FieldWidth = fieldSize.Width;
            FieldHeight = fieldSize.Height;
            Padding = new Padding(0, 0, 0, 0);
            Avoid = avoidPoints;
        }

        public AppleGen(Size fieldSize, IEnumerable<Point> avoidPoints, Padding p) : this(fieldSize, avoidPoints)
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
                int x = rnd.Next(Padding.Left + 1, this.FieldWidth - Padding.Right - 1);
                int y = rnd.Next(Padding.Top + 1, this.FieldHeight - Padding.Bottom - 1);
                point = new Point(x, y);
            } while (Avoid.ToList().Contains(point));

            return point;
        }
    }
}