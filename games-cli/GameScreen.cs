using System;
using System.Drawing;

namespace Games
{
    class GameScreen : Screen
    {
        Game game;
        Size windowSize;
        public override IDrawable[] ElementContent => getContent();

        public GameScreen(Game game, Size windowSize)
        {
            this.game = game;
            this.windowSize = windowSize;
            game.IsFocused = true;
        }

        private IDrawable[] getContent()
        {
            if (game.IsGameOver)
            {
                Exit(null);
                return new IDrawable[0];
            }

            var temp_drawer = new Drawer(windowSize.Width, windowSize.Height);
            game.PrepareForNextFrame(temp_drawer);
            game.NextFrame(temp_drawer);

            return temp_drawer.QueueForDrawing;
        }

        public override void HandleKey(ConsoleKey key)
        {
            game.HandleKey(key);
        }

        public override void OnWindowSizeChanged(int width, int height)
        {
            this.windowSize = new Size(width, height);
        }
    }
}