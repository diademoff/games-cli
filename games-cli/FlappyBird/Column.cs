using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class Column : IDrawableElement
    {
        public IDrawable[] ElementContent => getContent();

        int topLength;
        int bottomLength;
        private int locationX;
        private readonly int intervalHeight;
        private readonly int width;
        private readonly Size fieldSize;
        private readonly Padding p;

        public Column(int locationX, int intervalHeight, int width, Size fieldSize, Random rnd, Padding p)
        {
            this.locationX = locationX;
            this.intervalHeight = intervalHeight;
            this.width = width;
            this.fieldSize = fieldSize;
            this.p = p;

            int heighAvailable = fieldSize.Height - intervalHeight - p.Top - p.Bottom;
            topLength = rnd.Next(1, heighAvailable);
            bottomLength = heighAvailable - topLength;
        }

        public void MoveLeft(int pixels)
        {
            this.locationX -= pixels;
        }

        private IDrawable[] getContent()
        {
            // top left
            var tl = new Line('|', new Point(locationX, p.Top), new Point(locationX, p.Top + topLength));
            // top right
            var tr = new Line('|', new Point(locationX + width, p.Top), new Point(locationX + width, p.Top + topLength));
            // top bottom, нижняя черта верхней части колонки
            var tb = new Line('=', new Point(locationX, p.Top + topLength), new Point(locationX + width, p.Top + topLength));

            int b = fieldSize.Height - p.Bottom;
            // bottom left
            var bl = new Line('|', new Point(locationX, b), new Point(locationX, b - bottomLength));
            // bottom right
            var br = new Line('|', new Point(locationX + width, b), new Point(locationX + width, b - bottomLength));
            // bottom top, верхняя черта нижней части колонки
            var bt = new Line('=', new Point(locationX, b - bottomLength), new Point(locationX + width, b - bottomLength));

            List<IDrawable> res = new List<IDrawable>();

            res.AddRange(tl.ElementContent);
            res.AddRange(tr.ElementContent);
            res.AddRange(tb.ElementContent);

            res.AddRange(bl.ElementContent);
            res.AddRange(br.ElementContent);
            res.AddRange(bt.ElementContent);

            return res.ToArray();
        }
    }
}