using Games;
using System.Drawing;
using Xunit;

namespace unittests
{
    public class BorderTest
    {
        [Fact]
        public void CreateBorderNoPadding()
        {
            //*****
            //*   *
            //*****
            Border b = new Border('*', 5, 3, new Padding(0, 0, 0, 0));

            Assert.True(b.ElementContent.CheckDrawableLocations(new Point[]{
                new Point(0, 0),
                new Point(1, 0),
                new Point(2, 0),
                new Point(3, 0),
                new Point(4, 0),

                new Point(0, 1),
                new Point(0, 2),

                new Point(1, 2),
                new Point(2, 2),
                new Point(3, 2),
                new Point(4, 2),

                new Point(4, 1),
            }));
        }

        [Fact]
        public void CreateBorderLeftPadding()
        {
            // ****
            // *  *
            // ****
            Border b = new Border('*', 5, 3, new Padding(1, 0, 0, 0));

            Assert.True(b.ElementContent.CheckDrawableLocations(new Point[]{
                new Point(1, 0),
                new Point(2, 0),
                new Point(3, 0),
                new Point(4, 0),

                new Point(1, 1),
                new Point(1, 2),

                new Point(2, 2),
                new Point(3, 2),
                new Point(4, 2),

                new Point(4, 1),
            }));
        }

        [Fact]
        public void CreateBorderTopPadding()
        {
            //
            //*****
            //*****
            Border b = new Border('*', 5, 3, new Padding(0, 0, 1, 0));

            Assert.True(b.ElementContent.CheckDrawableLocations(new Point[]{
                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 1),
                new Point(3, 1),
                new Point(4, 1),

                new Point(0, 2),
                new Point(1, 2),
                new Point(2, 2),
                new Point(3, 2),
                new Point(4, 2)
            }));
        }

        [Fact]
        public void CreateBorderBottomPadding()
        {
            //*****
            //*****
            //
            Border b = new Border('*', 5, 3, new Padding(0, 0, 0, 1));

            Assert.True(b.ElementContent.CheckDrawableLocations(new Point[] {
                new Point(0, 0),
                new Point(1, 0),
                new Point(2, 0),
                new Point(3, 0),
                new Point(4, 0),

                new Point(0, 1),
                new Point(1, 1),
                new Point(2, 1),
                new Point(3, 1),
                new Point(4, 1)
            }));
        }
    }
}
