using System;
using System.Collections.Generic;
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

    // Нарисовать границу из символов
    public void CreateBorder(char c)
    {
        var lt = new Point(0, 0); // left top
        var rt = new Point(Width - 1, 0); // right top
        var lb = new Point(0, Height - 1); // left buttom
        var rb = new Point(Width - 1, Height - 1); // right buttom

        CreateBorder(c, lt, rt, lb, rb);
    }

    // Нарисовать границу из символов с отступами
    public void CreateBorder(char c, Padding p)
    {
        var lt = new Point(p.Left, p.Top); // left top
        var rt = new Point(Width - p.Right - 1, p.Top); // right top
        var lb = new Point(p.Left, Height - p.Buttom - 1); // left buttom
        var rb = new Point(Width - p.Right - 1, Height - p.Buttom - 1); // right buttom

        CreateBorder(c, lt, rt, lb, rb);
    }

    private void CreateBorder(char c, Point lt, Point rt, Point lb, Point rb)
    {
        // Верхняя граница
        CreateLine(c, lt, rt);
        // Левая граница
        CreateLine(c, lt, lb);
        // Правая граница
        CreateLine(c, rt, rb);
        // Нижняя граница
        CreateLine(c, lb, rb);
    }

    // Нарисовать элемент
    public void CreateSnake(IDrawableElement element)
    {
        foreach (IDrawable d in element.ElementContent)
        {
            CreateDrawable(d);
        }
    }

    // Удалить элемент
    public void RemoveSnake(IDrawableElement element)
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

    public void CreateDrawable(List<IDrawable> drawable)
    {
        foreach (var c in drawable)
        {
            CreateChar(c.Char, c.Location.X, c.Location.Y);
        }
    }

    public void RemoveDrawable(IDrawable drawable)
    {
        RemoveChar(drawable.Location.X, drawable.Location.Y);
    }

    public void RemoveDrawable(List<IDrawable> drawable)
    {
        foreach (var c in drawable)
        {
            RemoveChar(c.Location.X, c.Location.Y);
        }
    }

    // Нарисовать горизонтальную или вертикальную линию
    public void CreateLine(char c, Point p1, Point p2)
    {
        if (!(p1.X == p2.X || p1.Y == p2.Y))
        {
            throw new Exception("Можно нарисовать только вертикальную или горизонтальную линию. " +
            $"Координаты переданы: {p1.X} {p1.Y} и {p2.X} {p2.Y}");
        }

        if (p1.X == p2.X)
        {
            // Вертикальная прямая
            int from = Math.Min(p1.Y, p2.Y);
            int to = Math.Max(p1.Y, p2.Y);

            for (int i = from; i <= to; i++)
            {
                CreateChar(c, p1.X, i);
            }
        }
        else
        {
            // Горизонтальная прямая
            int from = Math.Min(p1.X, p2.X);
            int to = Math.Max(p1.X, p2.X);

            for (int i = from; i <= to; i++)
            {
                CreateChar(c, i, p1.Y);
            }
        }
    }
}