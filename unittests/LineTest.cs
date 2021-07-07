using Xunit;
using System.Drawing;
using Games;
using System;

namespace unittests
{
    public class LineTest
    {
        [Fact]
        public void CreateLine()
        {
            Line line_horizontal = new Line('*', new Point(0, 0), new Point(0, 3));
            Line line_vertical = new Line('+', new Point(0, 0), new Point(3, 0));

            for (int i = 0; i < 4; i++)
            {
                Assert.True(line_horizontal.ElementContent[i].Char == '*');
                Assert.True(line_horizontal.ElementContent[i].Location == new Point(0, i));

                Assert.True(line_vertical.ElementContent[i].Char == '+');
                Assert.True(line_vertical.ElementContent[i].Location == new Point(i, 0));
            }
        }

        [Fact]
        public void ThrowExceptionOnIncorrectLine()
        {
            Assert.Throws<Exception>(() => new Line('-', new Point(0, 0), new Point(1, 2)));
            Assert.Throws<Exception>(() => new Line('-', new Point(1, 1), new Point(0, 0)));
        }

        [Fact]
        public void LineOfOneSymbol()
        {
            Line line = new Line('#', new Point(5, 5), new Point(5, 5));

            Assert.True(line.ElementContent.Length == 1);
            Assert.True(line.ElementContent[0].Char == '#');
            Assert.True(line.ElementContent[0].Location.X == 5);
            Assert.True(line.ElementContent[0].Location.Y == 5);
        }
    }
}
