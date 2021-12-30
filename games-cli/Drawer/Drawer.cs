using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    /// <summary>
    /// Класс для рисования в консоли. Добавляете в очередь
    /// элементы и выводите их в консоль.
    /// </summary>
    public class Drawer
    {
        /// <summary>
        /// Массив содержит симолы которые отображены в консоли. Вместо
        /// изменения массива используйте очередь на отрисовку.
        /// </summary>
        public char[,] Content { get; private set; }

        /// <summary>
        /// В целях оптимизации вместо полной переотрисовки всего массива Content каждый кадр
        /// используется очередь на отрисовку. При следующей отрисовке символы
        /// отобразятся в консоли, а список с очередью будет очищен.
        /// </summary>
        private Dictionary<Point, DrawableChar> drawQueue = new Dictionary<Point, DrawableChar>();
        public IDrawable[] QueueForDrawing => drawQueue.Select(x => x.Value).ToArray();

        /// <summary>
        /// Высота и ширина. Этими значениями ограничится поле на
        /// котором можно рисовать.
        /// </summary>
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Drawer(int width, int height)
        {
            this.Content = new char[width, height];
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Отрисовать очередь. Вывести симолы в очереди.
        /// </summary>
        public void DrawToConsole()
        {
            foreach (KeyValuePair<Point, DrawableChar> p in drawQueue)
            {
                Content[p.Key.X, p.Key.Y] = p.Value.Char;
                DrawCharToConsole(p.Value.Char, p.Key);
            }
            drawQueue.Clear();
        }

        /// <summary>
        /// Перерисовать всё содержимое (High CPU usage)
        /// </summary>
        public void RedrawAll()
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

        /// <summary>
        ///  Отрисовать один символ в консоль
        /// </summary>
        private void DrawCharToConsole(char c, Point p)
        {
            c = c == (char)0 ? ' ' : c; // заменить пустой символ пробелом
            Content[p.X, p.Y] = c;
            Console.SetCursorPosition(p.X, p.Y);
            Console.Write(c);
        }

        /// <summary>
        /// Нарисовать границу из символов по краям с отступами.
        /// </summary>
        public Border CreateBorder(char c, Padding p)
        {
            Border b = new Border(c, Width, Height, p);
            Create(b);
            return b;
        }

        /// <summary>
        /// Добавить элемент в очередь на отрисовку.
        /// </summary>
        public void Create(IDrawableElement element)
        {
            foreach (IDrawable d in element.ElementContent)
            {
                Create(d);
            }
        }

        /// <summary>
        /// Добавить символ в очередь на отрисовку.
        /// </summary>
        public void Create(char c, int x, int y)
        {
            var p = new Point(x, y);
            var dc = new DrawableChar(c, new Point(x, y));
            if (!drawQueue.TryAdd(p, dc))
            {
                // Перезаписать значение если есть символ на этих координатах
                drawQueue[p] = dc;
            }
        }

        /// <summary>
        /// Добавить символ в очередь на отрисовку.
        /// </summary>
        public void Create(IDrawable drawable)
        {
            Create(drawable.Char, drawable.Location.X, drawable.Location.Y);
        }

        /// <summary>
        /// Добавить символы в очередь на отрисовку.
        /// </summary>
        public void Create(IEnumerable<IDrawable> drawables)
        {
            foreach (var item in drawables)
            {
                Create(item);
            }
        }

        /// <summary>
        /// Запросить затирание пробелами элемента. Запрос будет
        /// добавлен в очередь и удовлетворен во время следующей отрисовки.
        /// </summary>
        public void Remove(IDrawableElement element)
        {
            foreach (IDrawable d in element.ElementContent)
            {
                Remove(d.Location.X, d.Location.Y);
            }
        }

        /// <summary>
        /// Затереть пробелами символ.
        /// </summary>
        public void Remove(IDrawable drawable)
        {
            Remove(drawable.Location.X, drawable.Location.Y);
        }

        /// <summary>
        /// Затереть пробелами символы.
        /// </summary>
        public void Remove(IEnumerable<IDrawable> drawables)
        {
            foreach (var item in drawables)
            {
                Remove(item);
            }
        }

        /// <summary>
        /// Удалить символ по заданным координатам.
        /// </summary>
        public void Remove(int x, int y)
        {
            Create(' ', x, y);
        }
    }
}