using Games;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace unittests
{
    public static class Extensions
    {
        public static bool CheckDrawableLocations(this IDrawable[] input, Point[] locations)
        {
            HashSet<Point> array = new HashSet<Point>(input.Select(x => x.Location));
            HashSet<Point> points = new HashSet<Point>(locations);

            if (array.Count != points.Count)
                return false;

            foreach (var a in array)
            {
                if (!points.Any())
                    return false;
                if (!points.Contains(a))
                    return false;
                points.Remove(a);
            }

            return points.Count == 0;
        }
    }
}
