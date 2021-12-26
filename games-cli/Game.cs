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

        /// Пользователь поставил игру на паузу
        protected bool IsPaused = false;

        protected Size FieldSize;
        protected Padding Padding;

        /// Меню паузы с выбором действия
        protected SelectionMenu MenuPaused;
        /// Выбор действия после переполнения игрового поля
        protected SelectionMenu GameOverActionMenu;

        public Game(Size fieldSize, Padding p)
        {
            this.FieldSize = fieldSize;
            this.Padding = p;
        }

        protected SelectionMenu GetDefaultPauseMenu()
        {
            return new SelectionMenu(new string[]{
                "Resume",
                "Exit"
            }, FieldSize, 0, Padding);
        }

        protected SelectionMenu GetDefaultGameOverMenu()
        {
            return new SelectionMenu(new string[]{
                    "Restart",
                    "Exit"
            }, FieldSize, 0, Padding);
        }
    }
}