using System;

namespace Games
{
    public abstract class Game : IInteractive
    {
        public abstract bool IsFocused { get; set; }
        public abstract int DelayBetweenFrames { get; }
        public abstract bool IsGameOver { get; }
        /*
        Сформировать запросы на удаление старых символов/элементов
        */
        public abstract void PrepareForNextFrame(Drawer d);
        /*
        Сформировать запросы на отрисовку нужных элементов
        */
        public abstract void NextFrame(Drawer d);
        public abstract void HandleKey(ConsoleKey key);

        protected int FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT;
        protected Padding padding;

        public Game(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p)
        {
            this.FIELD_SIZE_HEIGHT = FIELD_SIZE_HEIGHT;
            this.FIELD_SIZE_WIDTH = FIELD_SIZE_WIDTH;
            this.padding = p;
        }
    }
}