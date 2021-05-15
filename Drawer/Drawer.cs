using System;
using System.Drawing;

/* Для рисования в консоли
Методы Draw* взаимодействуют с консолью
Методы Create и Remove изменяют содержимое Content
*/
class Drawer
{
    // Содержимое консоли
    // Все изменения помещаются в массив,
    // а при надобности отрисовываются
    public char[,] Content { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Drawer(int width, int height)
    {
        this.Content = new char[width, height];
        this.Width = width;
        this.Height = height;
    }

    // Отрисовать содержимое в консоль
    public void DrawAllToConsole()
    {
        for (int i = 0; i < Content.GetLength(0); i++)
        {
            for (int j = 0; j < Content.GetLength(1); j++)
            {
                var c = Content[i, j];
                var p = new Point(i, j);
                DrawCharToConsole(c, p);
            }
        }
    }

    // Отрисовать один символ в консоль
    public void DrawCharToConsole(char c, Point p)
    {
        c = c == (char)0 ? ' ' : c; // заменить пустой символ пробелом
        Content[p.X, p.Y] = c;
        Console.SetCursorPosition(p.X, p.Y);
        Console.Write(c);
    }

    // Нарисовать границу из символов с отступами
    public void CreateBorder(char c, Padding p)
    {
        Border b = new Border(c, Width, Height, p);
        Create(b);
    }

    // Нарисовать элемент
    public void Create(IDrawableElement element)
    {
        foreach (IDrawable d in element.ElementContent)
        {
            Create(d);
        }
    }

    // Поместить символ по координатам
    public void Create(char c, int x, int y)
    {
        Content[x, y] = c;
    }

    public void Create(IDrawable drawable)
    {
        Create(drawable.Char, drawable.Location.X, drawable.Location.Y);
    }

    // Удалить элемент
    public void Remove(IDrawableElement element)
    {
        foreach (IDrawable d in element.ElementContent)
        {
            Remove(d.Location.X, d.Location.Y);
        }
    }

    public void Remove(IDrawable drawable)
    {
        Remove(drawable.Location.X, drawable.Location.Y);
    }

    // Удалить символ по координатам (поставить пробел)
    public void Remove(int x, int y)
    {
        Content[x, y] = ' ';
    }
}