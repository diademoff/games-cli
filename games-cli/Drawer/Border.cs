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
            fromPoints(c, lt, rt, lb, rb);
        }

        public Border(char c, int width, int height, Padding p)
        {
            /*
            Создать ключевые точки с учетом отступов
            */

            var lt = new Point(p.Left, p.Top); // left top
            var rt = new Point(width - p.Right - 1, p.Top); // right top
            var lb = new Point(p.Left, height - p.Buttom - 1); // left buttom
            var rb = new Point(width - p.Right - 1, height - p.Buttom - 1); // right buttom

            fromPoints(c, lt, rt, lb, rb);
        }

        private void fromPoints(char c, Point lt, Point rt, Point lb, Point rb)
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
            {
                foreach (DrawableChar c in line.ElementContent)
                {
                    r.Add(c);
                }
            }
            return r.ToArray();
        }
    }
}