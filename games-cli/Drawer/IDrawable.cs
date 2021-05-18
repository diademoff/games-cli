/*
Символ, который можно вывеси в консоль
*/

using System.Drawing;

interface IDrawable
{
    char Char { get; }
    Point Location { get; }
}