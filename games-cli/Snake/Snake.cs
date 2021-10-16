using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class Snake : IDrawableElement
    {
        /**
        Змейка состоит за блоков
        */
        public List<DrawableChar> Blocks { get; private set; } = new List<DrawableChar>();
        /// Направление, в котором будет сделано следующее движение
        public Direction Direction { get; private set; }
        /// Символ, которым рисуется змейка
        public char SnakeChar { get; set; }
        public Point InitPosition => new Point(1, 1);
        /**
        Добавление нового блока к змейки происходит во время движения. Значение
        переменной показывает сколько еще блоков нужно добавить к змейке.
        */
        private int addBlockQueue = 0;
        public IDrawable[] ElementContent => GetContent();

        /**
        В каком направлении фактически было сделано движение последний раз. Так как за одну итерацию
        направление может сменится несколько раз.
        */
        Direction actualDirection;
        public Snake(char c, Padding p)
        {
            this.SnakeChar = c;
            this.Direction = Direction.Right;
            this.actualDirection = Direction.Right;
            this.Blocks.Add(new DrawableChar(c, new Point(p.Left + 1, p.Top + 1)));
        }

        /**
        Пересекает ли змейка границы
        */
        public bool BorderIntersect(Size fieldSize, Padding p)
        {
            var h = this.Blocks[0].Location;

            if (h.X <= p.Left || h.X >= fieldSize.Width - p.Right - 1 ||
                h.Y <= p.Top || h.Y >= fieldSize.Height - p.Bottom - 1)
            {
                return true;
            }
            return false;
        }

        /// Изменить направление змейки
        public void ChangeDirection(Direction dir)
        {
            // Проверить чтобы змейка не въехала в себя
            switch (dir)
            {
                case Direction.Up:
                    if (actualDirection != Direction.Down)
                        this.Direction = Direction.Up;
                    break;
                case Direction.Left:
                    if (actualDirection != Direction.Right)
                        this.Direction = Direction.Left;
                    break;
                case Direction.Right:
                    if (actualDirection != Direction.Left)
                        this.Direction = Direction.Right;
                    break;
                case Direction.Down:
                    if (actualDirection != Direction.Up)
                        this.Direction = Direction.Down;
                    break;
            }
        }

        /**
        Столкнулась ли змейка с собой или с краем.
        */
        public bool IsDead(Size FieldSize, Padding padding)
        {
            return this.SelfIntersect() || this.BorderIntersect(FieldSize, padding);
        }

        /// Сдвинуть змейку по направлению
        public void Move()
        {
            // Первый (ведущий) блок змейки
            DrawableChar head = Blocks[0];

            if (addBlockQueue > 0)
            {
                // Вместо добавления нового блока, просто
                // не удалять старый.
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
            actualDirection = this.Direction; // зафиксировать в каком направлении фактически двигается змейка

            DrawableChar blockToAdd = new DrawableChar(SnakeChar, newBlockLocation);
            Blocks.Insert(0, blockToAdd);
        }

        /**
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

        /// Находится ли голова змейки на яблоке
        public bool IsEaten(Apple apple)
        {
            return Blocks[0].Location.X == apple.Location.X &&
                    Blocks[0].Location.Y == apple.Location.Y;
        }

        /**
        Добавить блок к змейке
        */
        public void AddBlock()
        {
            this.addBlockQueue += 1;
        }

        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow || key == ConsoleKey.K)
            {
                ChangeDirection(Direction.Up);
            }
            else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow || key == ConsoleKey.H)
            {
                ChangeDirection(Direction.Left);
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow || key == ConsoleKey.L)
            {
                ChangeDirection(Direction.Right);
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow || key == ConsoleKey.J)
            {
                ChangeDirection(Direction.Down);
            }
        }

        /**
        Ковертировать блоки в IDrawable, чтобы реализовать интерфейс
        IDrawableElement
        */
        private IDrawable[] GetContent()
        {
            IDrawable[] r = new IDrawable[Blocks.Count];
            for (int i = 0; i < Blocks.Count; i++)
            {
                r[i] = (IDrawable)Blocks[i];
            }
            return r;
        }

        /**
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
}