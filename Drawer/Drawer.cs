using System;
using System.Drawing;

/* Для рисования в консоли
Методы Draw* взаимодействуют с консолью
Методы Create* и Remove* изменяют содержимое Content
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

    // Поместить символ по координатам
    public void CreateChar(char c, int x, int y)
    {
        Content[x, y] = c;
    }

    // Удалить символ по координатам (поставить пробел)
    public void RemoveChar(int x, int y)
    {
        Content[x, y] = ' ';
    }

    // Нарисовать границу из символов с отступами
    public void CreateBorder(char c, Padding p)
    {
        Border b = new Border(c, Width, Height, p);
        CreateElement(b);
    }

    // Нарисовать элемент
    public void CreateElement(IDrawableElement element)
    {
        foreach (IDrawable d in element.ElementContent)
        {
            CreateDrawable(d);
        }
    }

    // Удалить элемент
    public void RemoveElement(IDrawableElement element)
    {
        foreach (IDrawable d in element.ElementContent)
        {
            RemoveChar(d.Location.X, d.Location.Y);
        }
    }

    public void CreateDrawable(IDrawable drawable)
    {
        CreateChar(drawable.Char, drawable.Location.X, drawable.Location.Y);
    }

    public void RemoveDrawable(IDrawable drawable)
    {
        RemoveChar(drawable.Location.X, drawable.Location.Y);
    }
}