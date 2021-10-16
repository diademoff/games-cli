using System;

namespace Games
{
    /**
    Интерактивный - значит может обрабатывать нажатия клавиш
    */
    public interface IInteractive
    {
        /// Включена ли обработка нажатий
        bool IsFocused { get; set; }
        /// Обработать нажатие
        void HandleKey(ConsoleKey key);
    }
}