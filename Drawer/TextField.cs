// Текстовое поле
// Многострочность не поддерживается
using System.Drawing;

class TextField : IDrawableElement
{
    public string Text { get; set; }
    public IDrawable[] ElementContent => getContent();

    private Point startLocation;
    public int length { get; private set; }

    public TextField(Point startLocation, int length)
    {
        // позиция с которой начинать писать
        this.startLocation = startLocation;
        this.length = length;
        this.Text = "";
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
                // Если строка закончилась добавить пробел
                c = ' ';
            }
            r[i] = new DrawableChar(c, location);

        }
        return r;
    }
}

