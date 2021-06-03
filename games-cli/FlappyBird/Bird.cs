using System.Drawing;

namespace Games
{
    public class Bird : IDrawableElement
    {
        public IDrawable[] ElementContent => getContent();

        IDrawable[] fallingState = new[]
        {
            /*
            \   /
             \0/
            */
            new DrawableChar('\\', new Point(0, 0)),
            new DrawableChar('\\', new Point(1, 1)),
            new DrawableChar('0', new Point(2, 1)),
            new DrawableChar('/', new Point(3, 1)),
            new DrawableChar('/', new Point(4, 0))
        };

        IDrawable[] jumpingState = new[]
        {
            /*
             /0\
            /   \
            */
            new DrawableChar('/', new Point(0, 1)),
            new DrawableChar('/', new Point(1, 0)),
            new DrawableChar('0', new Point(2, 0)),
            new DrawableChar('\\', new Point(3, 0)),
            new DrawableChar('\\', new Point(4, 1))
        };

        /**
        У птички два состояния: падает или взлетает
        */
        bool falling = true;
        /**
        Высота, на которую взлетает птичка
        */
        int jumpSize;
        /*
        Положение птички
        */
        Point location;

        public Bird(int jumpSize, Point location)
        {
            this.jumpSize = jumpSize;
            this.location = location;
        }

        public void Jump()
        {
            this.location = new Point(this.location.X, this.location.Y - jumpSize);
            falling = false;
        }

        public void Fall(int offset)
        {
            this.location = new Point(this.location.X, this.location.Y + offset);
        }

        private IDrawable[] getContent()
        {
            IDrawable[] source;
            if (falling)
            {
                source = this.fallingState;
            }
            else
            {
                source = this.jumpingState;
                falling = true;
            }

            IDrawable[] result = new IDrawable[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                var t = source[i];
                result[i] = new DrawableChar(t.Char, new Point(t.Location.X + location.X, t.Location.Y + location.Y));
            }
            return result;
        }
    }
}