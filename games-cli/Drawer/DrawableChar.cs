/*
Простая реализация IDrawable - символ
*/
using System.Drawing;

namespace Games
{
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