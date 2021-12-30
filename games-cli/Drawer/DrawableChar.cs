using System.Drawing;

namespace Games
{
    /// <summary>
    /// Простая реализация IDrawable - символ
    /// </summary>
    public class DrawableChar : IDrawable
    {
        public char Char => c;
        char c;

        public Point Location => loc;
        Point loc;

        public DrawableChar(char c, Point loc)
        {
            this.c = c;
            this.loc = loc;
        }
    }
}