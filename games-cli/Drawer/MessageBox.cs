using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Текстовое сообщение в рамке. Состоит из рамки (Border) и текстового поля (TextField)
    */
    public class MessageBox : IDrawableElement
    {
        public IDrawable[] ElementContent => GetContent();
        Border border;
        TextField textField;
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
            Point textStartLocation = new Point((fieldWidth / 2) - (text.Length / 2),
                                    paddingTopBottom + (height / 2));

            this.border = new Border('+', fieldWidth, fieldHeight, paddingBorder);
            this.textField = new TextField(textStartLocation, text.Length);
            textField.Text = text;
        }

        private IDrawable[] GetContent()
        {
            List<IDrawable> r = new List<IDrawable>();

            foreach (var c in border.ElementContent)
            {
                r.Add(c);
            }

            foreach (var c in textField.ElementContent)
            {
                r.Add(c);
            }

            return r.ToArray();
        }
    }
}