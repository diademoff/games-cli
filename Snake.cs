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


class Snake
{
    public List<SnakeBlock> Blocks { get; private set; } = new List<SnakeBlock>();
    public Direction Direction { get; set; }
    public char SnakeChar { get; set; }

    /*
    Добавление нового блока к змейки происходит во время движения. Значение
    переменной показывает сколько еще блоков нужно добавить к змейке.
    */
    private int addBlockQueue = 0;

    public Snake(char c)
    {
        this.SnakeChar = c;
        this.Direction = Direction.Right;
        this.Blocks.Add(new SnakeBlock(c, new Point(1, 1)));
    }

    public Snake(char c, Direction dir, Point initPos)
    {
        this.SnakeChar = c;
        this.Direction = dir;
        this.Blocks.Add(new SnakeBlock(c, initPos));
    }

    // Сдвинуть змейку по направлению
    public void Move()
    {
        // Первый (ведущий) блок змейки
        SnakeBlock head = Blocks[0];

        if (addBlockQueue > 0)
        {
            addBlockQueue -= 1;
        }
        else
        {
            Blocks.RemoveAt(Blocks.Count - 1);
        }

        Point newBlockLocation = head.Location; // Координаты нового ведущего блока

        newBlockLocation = GetPosFollowingDirection(newBlockLocation, this.Direction);

        SnakeBlock blockToAdd = new SnakeBlock(SnakeChar, newBlockLocation);
        Blocks.Insert(0, blockToAdd);
    }

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

    public bool BorderIntersect(int field_width, int field_height)
    {
        var p = this.Blocks[0].Location;
        if (p.X == 0 || p.X == field_width - 1 ||
            p.Y == 0 || p.Y == field_height - 1)
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
}