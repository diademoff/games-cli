/*
Реализация IDrawableElement для вывода теста в консоль
*/
using System.Drawing;

namespace Games
{
    public class TextField : IDrawableElement
    {
        public string Text { get; set; }
        public IDrawable[] ElementContent => getContent();

        /*
        Позиция, начиная с которой будет напечатан текст слева направо
        */
        private Point startLocation;
        // Максимальная длина текстового поля
        public int length { get; private set; }

        public TextField(Point startLocation, int length, string text = "")
        {
            // Позиция с которой начинать писать строку
            this.startLocation = startLocation;
            this.length = length;
            this.Text = text;
        }

        private IDrawable[] getContent()
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