using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    /// <summary>
    /// Меню с вариантами выбора
    /// </summary>
    public class SelectionMenu : IDrawableElement, IInteractive
    {
        public IDrawable[] ElementContent => GetContent();
        /// <summary>
        /// Выбран ли како-нибудь вариант
        /// </summary>
        public bool IsSelected = false;
        public bool IsFocused { get; set; } = false;
        /// <summary>
        /// Выбранный вариант
        /// </summary>
        public int SelectedIndex { get; private set; }
        Border Border;
        TextField[] Variants => GetTextFields(strVariants, SelectedIndex);

        string[] strVariants;
        /// <summary>
        /// Отступ сверху и снизу (отступы одинаковы)
        /// </summary>
        int paddingTopBottom;
        /// <summary>
        /// Отступ слева и справа
        /// </summary>
        int paddingLeftRight;
        int fieldWidth;
        int fieldHeight;

        int defaultSelected;

        int menuHeight;
        int menuWidth;

        public SelectionMenu(string[] strVariants, Size fieldSize, int defaultSelected, Padding p)
        {
            this.SelectedIndex = defaultSelected;
            this.fieldWidth = fieldSize.Width;
            this.fieldHeight = fieldSize.Height;
            this.strVariants = strVariants;
            this.defaultSelected = defaultSelected;

            fieldWidth -= (p.Left + p.Right);
            fieldHeight -= (p.Bottom + p.Top);

            // Длина самого длинного слова
            int maxVariantLength = strVariants.OrderByDescending(n => n.Length).First().Length + 2;

            this.menuHeight = (strVariants.Length * 2) + 2;
            this.menuWidth = maxVariantLength + 4;

            this.paddingTopBottom = (fieldHeight - menuHeight) / 2;
            this.paddingLeftRight = (fieldWidth - menuWidth) / 2;

            /*
            Расчитать отступы таким образом чтобы меню было по середине
            */
            Padding paddingBorder = new Padding(paddingLeftRight, paddingLeftRight,
                                paddingTopBottom, paddingTopBottom);

            this.Border = new Border('+', fieldWidth, fieldHeight, paddingBorder);
        }

        public void ChangeVariantText(int index, string value)
        {
            this.strVariants[index] = value;
        }

        /// <summary>
        /// Сбросить сделанный выбор, вернуть в начальное состояние
        /// </summary>
        public void Reuse()
        {
            this.SelectedIndex = defaultSelected;
            this.IsSelected = false;
        }

        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.W || key == ConsoleKey.K || key == ConsoleKey.UpArrow)
            {
                if (SelectedIndex == 0)
                {
                    SelectedIndex = strVariants.Length - 1;
                    return;
                }
                SelectedIndex -= 1;
            }
            else if (key == ConsoleKey.S || key == ConsoleKey.J || key == ConsoleKey.DownArrow)
            {
                if (SelectedIndex == strVariants.Length - 1)
                {
                    SelectedIndex = 0;
                    return;
                }
                SelectedIndex += 1;
            }
            else if (key == ConsoleKey.Enter || key == ConsoleKey.Spacebar)
            {
                IsSelected = true;
            }
        }

        private TextField[] GetTextFields(string[] variants, int selectedIndex)
        {
            TextField[] fields = new TextField[variants.Length];
            for (int i = 0; i < variants.Length; i++)
            {
                string text = variants[i];

                // добавить пробелы чтобы затереть старые символы
                if (i == selectedIndex)
                {
                    int l = ((this.menuWidth - text.Length - 2) / 2) + 1;
                    l = l < 0 ? 0 : l; // length is not less than zero
                    string whiteSpace = new string(' ', l);
                    text = $"{whiteSpace}>{text}<{whiteSpace}";
                }
                else
                {
                    int l = (this.menuWidth - text.Length - 2) / 2;
                    string whiteSpace = new string(' ', l);
                    text = $"{whiteSpace}{text}{whiteSpace}";
                }

                Point textStartLocation = new Point((fieldWidth / 2) - (text.Length / 2),
                                    paddingTopBottom + (i * 2) + 2);

                TextField textField = new TextField(textStartLocation, text.Length, text);
                fields[i] = textField;
            }

            return fields;
        }

        private IDrawable[] GetContent()
        {
            return Variants.SelectMany(x => x.ElementContent).Concat(Border.ElementContent).ToArray();
        }
    }
}