/*
Яблоко, которое змейка должна съесть
*/

using System;
using System.Drawing;
using System.Linq;

class Apple : IDrawable
{
    public Point Location { get; private set; }
    public char Char { get; set; } = '☼';

    // Создать яблоко в поле с случайным расположением
    public Apple(AppleGen applegen, ref Random rnd)
    {
        this.Location = applegen.GetRandomPoint(ref rnd);
    }
}

struct AppleGen
{
    public int Field_width;
    public int Field_height;
    public Point[] Avoid; // Не генерировать в заданных точках

    public AppleGen(int field_width, int field_height, Snake snake)
    {
        Field_width = field_width;
        Field_height = field_height;

        Avoid = new Point[snake.Blocks.Count];

        for (int i = 0; i < snake.Blocks.Count; i++)
        {
            Avoid[i] = snake.Blocks[i].Location; // не генерировать на змейке
        }
    }

    public Point GetRandomPoint(ref Random rnd)
    {
        Point point;

        do
        {
            point =  new Point(rnd.Next(this.Field_width), rnd.Next(this.Field_height));
        } while (Avoid.ToList().Contains(point));

        return point;
    }
}