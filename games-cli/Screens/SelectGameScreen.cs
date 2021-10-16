using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    class SelectGameScreen : Screen
    {
        public override IDrawable[] ElementContent => sm.ElementContent;
        List<IInteractive> keyHandlers = new List<IInteractive>();
        string[] variants;
        SelectionMenu sm;
        Padding p;

        public SelectGameScreen(string[] variants, Size fieldSize, Padding p)
        {
            this.p = p;
            this.variants = variants;
            OnWindowSizeChanged(fieldSize);
        }

        public override void HandleKey(ConsoleKey key)
        {
            sm.HandleKey(key);
            if (sm.IsSelected)
                Exit(sm.SelectedIndex);
        }

        public override void OnWindowSizeChanged(Size fieldSize)
        {
            if (keyHandlers.Contains(sm))
                keyHandlers.Remove(sm);

            sm = new SelectionMenu(variants, fieldSize, defaultSelected: 0, p);
            sm.IsFocused = true;
            keyHandlers.Add(sm);
        }
    }
}