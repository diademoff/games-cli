using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    /// <summary>
    /// Текстовое сообщение в рамке. Состоит из рамки (Border) и текстового поля (TextField)
    /// </summary>
    public class MessageBox : IDrawableElement
    {
        public IDrawable[] ElementContent => GetContent();
        Border border;
        Func<Point> textStartLocation;
        TextField textField => new TextField(textStartLocation(), Text.Length) { Text = Text };
        public string Text { get; set; }

        public MessageBox(string text, int width, int height, int fieldWidth, int fieldHeight, Padding p)
        {
            fieldWidth -= (p.Left + p.Right);
            fieldHeight -= (p.Bottom + p.Top);

            this.Text = text;
            int paddingTopBottom = (fieldHeight - height) / 2;
            int paddingLeftRight = (fieldWidth - width) / 2;

            /*
            Расчитать отступы таким образом сообщение было по середине
            */
            Padding paddingBorder = new Padding(paddingLeftRight, paddingLeftRight,
                                paddingTopBottom, paddingTopBottom);

            /*
            Расчитать координаты текста так чтобы он был в центе обводки
            */
            textStartLocation = () => new Point((fieldWidth / 2) - (Text.Length / 2),
                                        paddingTopBottom + (height / 2));

            this.border = new Border('+', fieldWidth, fieldHeight, paddingBorder);
        }

        private IDrawable[] GetContent()
        {
            return border.ElementContent.Concat(textField.ElementContent).ToArray();
        }
    }
}