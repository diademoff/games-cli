using System.Drawing;

/*
Символ, который можно вывеси в консоль
*/
interface IDrawable
{
    char Char { get; }
    Point Location { get; }
}