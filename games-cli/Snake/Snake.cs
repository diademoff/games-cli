using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class Snake : IDrawableElement
    {
        public List<SnakeBlock> Blocks { get; private set; } = new List<SnakeBlock>();
        public Direction Direction { get; private set; }
        public char SnakeChar { get; set; }
        public Point InitPosition => new Point(1, 1);
        /*
        Добавление нового блока к змейки происходит во время движения. Значение
        переменной показывает сколько еще блоков нужно добавить к змейке.
        */
        private int addBlockQueue = 0;
        public IDrawable[] ElementContent => getContent();

        /*
        В каком направлении фактически было сделано движение последний раз. Так как за одну итерацию
        направление может сменится два раза.
        */
        Direction actual_direction;
        public Snake(char c, Padding p)
        {
            this.SnakeChar = c;
            this.Direction = Direction.Right;
            this.actual_direction = Direction.Right;
            this.Blocks.Add(new SnakeBlock(c, new Point(p.Left + 1, p.Top + 1)));
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

        // Изменить направление змейки
        public void ChangeDirection(Direction dir)
        {
            // Проверить чтобы змейка не въехала в себя
            if (dir == Direction.Up)
            {
                if (actual_direction != Direction.Down)
                {
                    this.Direction = Direction.Up;
                }
            }
            else if (dir == Direction.Left)
            {
                if (actual_direction != Direction.Right)
                {
                    this.Direction = Direction.Left;
                }
            }
            else if (dir == Direction.Right)
            {
                if (actual_direction != Direction.Left)
                {
                    this.Direction = Direction.Right;
                }
            }
            else if (dir == Direction.Down)
            {
                if (actual_direction != Direction.Up)
                {
                    this.Direction = Direction.Down;
                }
            }
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
            actual_direction = this.Direction; // зафиксировать в каком направлении фактически двигается змейка

            SnakeBlock blockToAdd = new SnakeBlock(SnakeChar, newBlockLocation);
            Blocks.Insert(0, blockToAdd);
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

        // Находится ли голова змейки на яблоке
        public bool IsEaten(Apple apple)
        {
            return Blocks[0].Location.X == apple.Location.X &&
                    Blocks[0].Location.Y == apple.Location.Y;
        }

        /*
        Добавить блок к змейке
        */
        public void AddBlock()
        {
            this.addBlockQueue += 1;
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
}