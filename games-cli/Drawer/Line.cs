using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Горизонтальная или вертикальная линия
    */
    public class Line : IDrawableElement
    {
        public IDrawable[] ElementContent => lineChars.ToArray();
        List<DrawableChar> lineChars = new List<DrawableChar>();

        /**
        Создать линию по двум точкам
        */
        public Line(char c, Point p1, Point p2)
        {
            if (!(p1.X == p2.X || p1.Y == p2.Y))
            {
                throw new Exception("Можно нарисовать только вертикальную или горизонтальную линию. " +
                $"Координаты переданы: {p1.X} {p1.Y} и {p2.X} {p2.Y}");
            }

            if (p1.X == p2.X)
            {
                // Вертикальная прямая
                int from = Math.Min(p1.Y, p2.Y);
                int to = Math.Max(p1.Y, p2.Y);

                for (int i = from; i <= to; i++)
                {
                    lineChars.Add(new DrawableChar(c, new Point(p1.X, i)));
                }
            }
            else
            {
                // Горизонтальная прямая
                int from = Math.Min(p1.X, p2.X);
                int to = Math.Max(p1.X, p2.X);

                for (int i = from; i <= to; i++)
                {
                    lineChars.Add(new DrawableChar(c, new Point(i, p1.Y)));
                }
            }
        }
    }
}