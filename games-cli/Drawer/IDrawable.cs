using System.Drawing;

namespace Games
{
    /**
    Символ, который можно вывеси в консоль
    */
    public interface IDrawable
    {
        char Char { get; }
        Point Location { get; }
    }
}