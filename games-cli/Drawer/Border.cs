using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Граница/обводка чего-либо
    */
    public class Border : IDrawableElement
    {
        public IDrawable[] ElementContent => getContent();
        List<Line> border_lines = new List<Line>();

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

        /**Как работают отступы:
         - Создается контейнер нужного размера, например если граница 8x4,
           то создается контейнер 8 на 4.

         - Если отступ 1 сверху, то верхняя граница смещается:
            ********
            *      *  ->  ********
            *      *  ->  *      *
            ********      ********

         - Если отступ сверху 1 и слева 3:
            ********
            *      *  ->     *****
            *      *  ->     *   *
            ********         *****

         - Отступ справа 4:
            ********      ****
            *      *  ->  *  *
            *      *  ->  *  *
            ********      ****
        */
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

            border_lines.Add(top);
            border_lines.Add(left);
            border_lines.Add(right);
            border_lines.Add(button);
        }

        private IDrawable[] getContent()
        {
            List<DrawableChar> r = new List<DrawableChar>();
            foreach (Line line in border_lines)
                foreach (DrawableChar c in line.ElementContent)
                    r.Add(c);
            return r.ToArray();
        }
    }
}