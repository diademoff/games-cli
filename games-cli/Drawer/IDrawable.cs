/*
Символ, который можно вывеси в консоль
*/
using System.Drawing;

namespace Games
{
    public interface IDrawable
    {
        char Char { get; }
        Point Location { get; }
    }
}