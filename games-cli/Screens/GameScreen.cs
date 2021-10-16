using System;
using System.Drawing;

namespace Games
{
    class GameScreen : Screen
    {
        Game game;
        Size FieldSize;
        public override IDrawable[] ElementContent => GetContent();

        public GameScreen(Game game, Size windowSize)
        {
            this.game = game;
            this.FieldSize = windowSize;
            game.IsFocused = true;
        }

        private IDrawable[] GetContent()
        {
            if (game.IsGameOver)
            {
                Exit(null);
                return new IDrawable[0];
            }

            var tempDrawer = new Drawer(FieldSize.Width, FieldSize.Height);
            game.PrepareForNextFrame(tempDrawer);
            game.NextFrame(tempDrawer);

            return tempDrawer.QueueForDrawing;
        }

        public override void HandleKey(ConsoleKey key)
        {
            game.HandleKey(key);
        }

        public override void OnWindowSizeChanged(Size fieldSize)
        {
            this.FieldSize = fieldSize;
        }
    }
}