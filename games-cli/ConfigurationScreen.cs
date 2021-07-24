using System;
using System.Drawing;

namespace Games
{
    class ConfigurationScreen : Screen
    {
        ConfigMenu cm;
        Padding p;
        ConfigStorage newConfig;
        public override IDrawable[] ElementContent => cm.ElementContent;

        public ConfigurationScreen(Size windowSize, Padding p)
        {
            newConfig = ConfigStorage.Current;
            OnWindowSizeChanged(windowSize.Width, windowSize.Height);
            this.p = p;
            this.cm.IsFocused = true;
        }

        public override void HandleKey(ConsoleKey key)
        {
            cm.HandleKey(key);

            if (cm.IsDone)
                Exit(newConfig);
        }

        public override void OnWindowSizeChanged(int width, int height)
        {
            cm = new ConfigMenu(
                newConfig.GetConfigParams(),
                new Size(width, height),
                p
            );
        }
    }
}