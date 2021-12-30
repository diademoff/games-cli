using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /// <summary>
    /// Граница/обводка чего-либо
    /// </summary>
    public class Border : IDrawableElement
    {
        public IDrawable[] ElementContent => GetContent();
        List<Line> borderLines = new List<Line>();

        public Border(char c, Point lt, Point rt, Point lb, Point rb)
        {
            FromPoints(c, lt, rt, lb, rb);
        }

        public Border(char c, int width, int height, Padding p)
        {
            /*
            Создать ключевые точки с учетом отступов
            */

            // Convert to location index
            width -= 1;
            height -= 1;

            var lt = new Point(0, 0); // left top
            var rt = new Point(width, 0); // right top
            var lb = new Point(0, height); // left bottom
            var rb = new Point(width, height); // right bottom

            ApplyPaddings(p, ref lt, ref rt, ref lb, ref rb);

            FromPoints(c, lt, rt, lb, rb);
        }

        /// <summary>
        /// Как работают отступы:
        /// - Создается контейнер нужного размера, например если граница 8x4,
        ///   то создается контейнер 8 на 4.
        ///
        /// - Если отступ 1 сверху, то верхняя граница смещается:
        ///    ********
        ///    *      *  ->  ********
        ///    *      *  ->  *      *
        ///    ********      ********
        ///
        /// - Если отступ сверху 1 и слева 3:
        ///    ********
        ///    *      *  ->     *****
        ///    *      *  ->     *   *
        ///    ********         *****
        ///
        /// - Отступ справа 4:
        ///    ********      ****
        ///    *      *  ->  *  *
        ///    *      *  ->  *  *
        ///    ********      ****
        /// </summary>
        private void ApplyPaddings(Padding p, ref Point lt, ref Point rt, ref Point lb, ref Point rb)
        {
            lt.X += p.Left;
            rt.X -= p.Right;
            lb.X += p.Left;
            rb.X -= p.Right;

            lt.Y += p.Top;
            rt.Y += p.Top;
            lb.Y -= p.Bottom;
            rb.Y -= p.Bottom;
        }

        private void FromPoints(char c, Point lt, Point rt, Point lb, Point rb)
        {
            Line top = new Line(c, lt, rt);
            Line left = new Line(c, lt, lb);
            Line right = new Line(c, rt, rb);
            Line button = new Line(c, lb, rb);

            borderLines.Add(top);
            borderLines.Add(left);
            borderLines.Add(right);
            borderLines.Add(button);
        }

        private IDrawable[] GetContent()
        {
            List<DrawableChar> r = new List<DrawableChar>();
            foreach (Line line in borderLines)
                foreach (DrawableChar c in line.ElementContent)
                    r.Add(c);
            return r.ToArray();
        }
    }
}