using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class TetrominoO : Tetromino
    {
        public TetrominoO(int field_width, int init_y_location) : base(field_width, init_y_location)
        {
        }

        protected override List<Point[]> structureRotate
        {
            get
            {
                return new List<Point[]> {
                    // Это квадрат, как ни крути :)
                    // Поэтому в списке только один массив
                    new Point[]{
                        new Point(0, 0),
                        new Point(0, 1),
                        new Point(1, 0),
                        new Point(1, 1),
                    }
                };
            }
        }
    }
}