using System.Drawing;

namespace Games
{
    /// <summary>
    /// Символ который можно вывеси в консоль.
    /// </summary>
    public interface IDrawable
    {
        char Char { get; }
        Point Location { get; }
    }
}