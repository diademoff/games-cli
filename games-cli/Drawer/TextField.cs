using System.Drawing;

namespace Games
{
    /// <summary>
    /// Реализация IDrawableElement для вывода теста в консоль
    /// </summary>
    public class TextField : IDrawableElement
    {
        public string Text { get; set; }
        public IDrawable[] ElementContent => GetContent();

        /// <summary>
        /// Позиция, начиная с которой будет напечатан текст слева направо
        /// </summary>
        private Point startLocation;
        /// <summary>
        /// Максимальная длина текстового поля
        /// </summary>
        public int length { get; private set; }

        public TextField(Point startLocation, int length, string text = "")
        {
            // Позиция с которой начинать писать строку
            this.startLocation = startLocation;
            this.length = length;
            this.Text = text;
        }

        private IDrawable[] GetContent()
        {
            IDrawable[] r = new IDrawable[length];
            for (int i = 0; i < length; i++)
            {
                Point location = new Point(startLocation.X + i, startLocation.Y);
                char c;
                if (Text.Length - 1 >= i)
                {
                    // Добавить символ из строки
                    c = Text[i];
                }
                else
                {
                    /*
                    Если строка закончилась заполнить оставшееся
                    место пробелами
                    */
                    c = ' ';
                }
                r[i] = new DrawableChar(c, location);
            }
            return r;
        }
    }
}