using System;
using System.Collections.Generic;
using System.Drawing;

struct SnakeBlock : IDrawable
{
    public Point Location { get; private set; }
    public char Char { get; private set; }

    public SnakeBlock(char c, Point p)
    {
        this.Location = p;
        this.Char = c;
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right,
    None
}

class Snake : IDrawableElement, IInteractive
{
    public List<SnakeBlock> Blocks { get; private set; } = new List<SnakeBlock>();
    public Direction Direction { get; set; }
    public char SnakeChar { get; set; }

    public Point Position => new Point(1, 1);
    public IDrawable[] ElementContent => getContent();

    /*
    Добавление нового блока к змейки происходит во время движения. Значение
    переменной показывает сколько еще блоков нужно добавить к змейке.
    */
    private int addBlockQueue = 0;

    public bool IsFocused { get => isFocused; set => isFocused = value; }
    bool isFocused = true;

    public Snake(char c)
    {
        this.SnakeChar = c;
        this.Direction = Direction.Right;
        this.Blocks.Add(new SnakeBlock(c, new Point(1, 1)));
    }

    // Сдвинуть змейку по направлению
    public void Move()
    {
        // Первый (ведущий) блок змейки
        SnakeBlock head = Blocks[0];

        if (addBlockQueue > 0)
        {
            // Вместо добавления, не удаляется
            addBlockQueue -= 1;
        }
        else
        {
            Blocks.RemoveAt(Blocks.Count - 1);
        }

        /*
        Для смещения последий блок перемещается в начало, перед текущим
        ведущим.
        */

        Point newBlockLocation = head.Location; // Координаты нового ведущего блока

        newBlockLocation = GetPosFollowingDirection(newBlockLocation, this.Direction);

        SnakeBlock blockToAdd = new SnakeBlock(SnakeChar, newBlockLocation);
        Blocks.Insert(0, blockToAdd);
    }

    public void HandleKey(ConsoleKey key)
    {
        if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
        {
            this.Direction = Direction.Up;
        }
        else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
        {
            this.Direction = Direction.Left;
        }
        else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
        {
            this.Direction = Direction.Right;
        }
        else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow)
        {
            this.Direction = Direction.Down;
        }
    }

    /*
    Добавить блок к змейке
    */
    public void AddBlock()
    {
        this.addBlockQueue += 1;
    }

    // Находится ли голова змейки на яблоке
    public bool IsEaten(Apple apple)
    {
        return Blocks[0].Location.X == apple.Location.X &&
                Blocks[0].Location.Y == apple.Location.Y;
    }

    /*
    Пересекает ли змейка сама себя в данный момент
    */
    public bool SelfIntersect()
    {
        for (int i = 0; i < Blocks.Count - 1; i++)
        {
            for (int j = i + 1; j < Blocks.Count; j++)
            {
                if (Blocks[i].Location == Blocks[j].Location)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /*
    Пересекает ли змейка границы
    */
    public bool BorderIntersect(int field_width, int field_height, Padding p)
    {
        var h = this.Blocks[0].Location;

        if (h.X <= p.Left || h.X >= field_width - p.Right - 1 ||
            h.Y <= p.Top || h.Y >= field_height - p.Buttom - 1)
        {
            return true;
        }
        return false;
    }

    /*
    Получить координаты левее/правее/ниже/выше на 1 чем заданная
    */
    private Point GetPosFollowingDirection(Point point, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                point.Y -= 1;
                break;
            case Direction.Down:
                point.Y += 1;
                break;
            case Direction.Left:
                point.X -= 1;
                break;
            case Direction.Right:
                point.X += 1;
                break;
            case Direction.None:
                // ничего не делать
                break;
        }
        return point;
    }

    /*
    Ковертировать блоки в IDrawable, чтобы реализовать интерфейс
    IDrawableElement
    */
    private IDrawable[] getContent()
    {
        IDrawable[] r = new IDrawable[Blocks.Count];
        for (int i = 0; i < Blocks.Count; i++)
        {
            r[i] = (IDrawable)Blocks[i];
        }
        return r;
    }
}