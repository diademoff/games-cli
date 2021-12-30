using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    /// <summary>
    /// Яблоко, которое змейка должна съесть
    /// </summary>
    public class Apple : IDrawable
    {
        public Point Location { get; private set; }
        public char Char => ConfigStorage.Current.SnakeGameAppleChar.Value;

        /// <summary>
        /// Создать яблоко в поле с случайным расположением
        /// </summary>
        public Apple(AppleGen applegen, ref Random rnd)
        {
            this.Location = applegen.GetRandomPoint(ref rnd);
        }
    }

    /// <summary>
    /// Apple generator
    /// </summary>
    public struct AppleGen
    {
        public int FieldWidth;
        public int FieldHeight;
        /// <summary>
        /// Не генерировать в заданных точках (на теле змейки)
        /// </summary>
        private IEnumerable<Point> Avoid;
        /// <summary>
        /// Учитывать отступы
        /// </summary>
        public Padding Padding;

        /// <summary>
        /// Генератор яблок
        /// </summary>
        /// <param name="fieldSize">Размер поля</param>
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

        /// <summary>
        /// Сгенерировать место в соответствии с заданными параметрами.
        /// </summary>
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