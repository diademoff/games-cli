using Xunit;
using System.Drawing;
using Games;

namespace UnitTests
{
    public class UnitTests
    {
        [Fact]
        public void CreateLineTest()
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
    }
}
