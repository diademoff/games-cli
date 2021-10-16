using System;
using System.Drawing;

namespace Games
{
    public abstract class Game : IInteractive
    {
        public abstract bool IsFocused { get; set; }
        public abstract int DelayBetweenFrames { get; }
        public abstract bool IsGameOver { get; }
        /**
        Сформировать запросы на удаление старых символов/элементов
        */
        public abstract void PrepareForNextFrame(Drawer d);
        /**
        Сформировать запросы на отрисовку нужных элементов
        */
        public abstract void NextFrame(Drawer d);
        public abstract void HandleKey(ConsoleKey key);

        protected Size FieldSize;
        protected Padding padding;

        public Game(Size fieldSize, Padding p)
        {
            this.FieldSize = fieldSize;
            this.padding = p;
        }
    }
}