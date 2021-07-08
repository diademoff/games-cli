using Games;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using Xunit;

namespace unittests
{
    public class MessageBoxTest
    {
        class DrawableCompare : IEqualityComparer<IDrawable>
        {
            public bool Equals(IDrawable x, IDrawable y) => x.Location == y.Location;
            public int GetHashCode([DisallowNull] IDrawable obj) => obj.Location.X + obj.Location.Y;
        }

        [Fact]
        public void CreateMessageTest()
        {
            //+++++++++++
            //+         +
            //+ Message +
            //+         +
            //+++++++++++
            MessageBox msg = new MessageBox("Message", 11, 5, 11, 5, new Padding(0, 0, 0, 0));

            // Символы сообщения:
            // [расположение] -> символ
            Dictionary<Point, char> chars = new Dictionary<Point, char>();
            msg.ElementContent.Distinct(new DrawableCompare()).ToList().ForEach(x => chars.Add(x.Location, x.Char));

            // Индексы букв сообщения
            Point[] messageLocation = new Point[]
            {
                new Point(2, 2),
                new Point(3, 2),
                new Point(4, 2),
                new Point(5, 2),
                new Point(6, 2),
                new Point(7, 2),
                new Point(8, 2),
            };

            // Проверить что сообщение в нужном месте
            for (int i = 0; i < messageLocation.Length; i++)
                Assert.True(chars[messageLocation[i]] == "Message"[i]);

            // Остальные символы это '+'
            foreach (KeyValuePair<Point, char> pair in chars)
            {
                if (messageLocation.Contains(pair.Key))
                    continue;
                Assert.True(pair.Value == '+');
            }
        }
    }
}
