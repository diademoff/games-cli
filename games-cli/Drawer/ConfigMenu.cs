using System;
using System.Drawing;
using System.Linq;

namespace Games
{
    /**
    Реализация IDrawableElement с настройками приложения
    */
    class ConfigMenu : IDrawableElement, IInteractive
    {
        public bool IsFocused { get; set; }
        public bool IsDone { get; private set; }
        SelectionMenu configMenu;
        public IDrawable[] ElementContent => configMenu.ElementContent;
        public ConfigParam[] ConfigParams { get; private set; }

        public ConfigMenu(ConfigParam[] configParams, Size fieldSize, Padding p)
        {
            this.ConfigParams = configParams;
            configMenu = new SelectionMenu(
                configParams.Select(x => x.ToString()).ToArray(),
                fieldSize,
                defaultSelected: 0,
                p
            );
        }

        public void Reuse()
        {
            this.IsDone = false;
        }

        public void HandleKey(ConsoleKey key)
        {
            int si = configMenu.SelectedIndex;

            if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
                configMenu.ChangeVariantText(si, ConfigParams[si].SelectPrevValue());
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
                configMenu.ChangeVariantText(si, ConfigParams[si].SelectNextValue());
            else if (key == ConsoleKey.Enter || key == ConsoleKey.Escape)
                this.IsDone = true;
            else
                configMenu.HandleKey(key);
        }
    }
}