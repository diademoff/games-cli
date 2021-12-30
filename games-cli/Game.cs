using System;
using System.Drawing;

namespace Games
{
    public abstract class Game : IInteractive
    {
        public abstract bool IsFocused { get; set; }
        public abstract int DelayBetweenFrames { get; }
        public abstract bool IsGameOver { get; }
        /// <summary>
        /// Сформировать запросы на удаление старых символов/элементов
        /// </summary>
        public abstract void PrepareForNextFrame(Drawer d);
        /// <summary>
        /// Сформировать запросы на отрисовку нужных элементов
        /// </summary>
        public abstract void NextFrame(Drawer d);
        public abstract void HandleKey(ConsoleKey key);

        /// <summary>
        /// Пользователь поставил игру на паузу
        /// </summary>
        protected bool IsPaused = false;

        protected Size FieldSize;
        protected Padding Padding;

        /// <summary>
        /// Меню паузы с выбором действия
        /// </summary>
        protected SelectionMenu MenuPaused;
        /// <summary>
        /// Выбор действия после переполнения игрового поля
        /// </summary>
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