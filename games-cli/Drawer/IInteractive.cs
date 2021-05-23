using System;

namespace Games
{
    /**
    Интерактивный - значит может обрабатывать нажатия клавиш
    */
    public interface IInteractive
    {
        /// Есть ли фокус
        bool IsFocused { get; set; }
        /// Обработать нажатие
        void HandleKey(ConsoleKey key);
    }
}