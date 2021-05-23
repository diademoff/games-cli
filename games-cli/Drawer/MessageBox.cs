using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    /**
    Текстовое сообщение в рамке. Состоит из рамки (Border) и текстового поля (TextField)
    */
    public class MessageBox : IDrawableElement
    {
        public IDrawable[] ElementContent => getContent();
        /**
        Рамка
        */
        Border border;
        /// Текстовое поле
        TextField textField;
        public string Text { get; set; }

        public MessageBox(string text, int width, int height, int field_width, int field_height, Padding p)
        {
            field_width -= (p.Left + p.Right);
            field_height -= (p.Buttom + p.Top);

            this.Text = text;
            int padding_topbuttom = (field_height - height) / 2;
            int padding_leftright = (field_width - width) / 2;

            /*
            Расчитать отступы таким образом сообщение было по середине
            */
            Padding p_border = new Padding(padding_leftright, padding_leftright,
                                padding_topbuttom, padding_topbuttom);

            /*
            Расчитать координаты текста так чтобы он был в центе обводки
            */
            Point textStartLocation = new Point((field_width / 2) - (text.Length / 2),
                                    padding_topbuttom + (height / 2));

            this.border = new Border('+', field_width, field_height, p_border);
            this.textField = new TextField(textStartLocation, text.Length);
            textField.Text = text;
        }

        private IDrawable[] getContent()
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