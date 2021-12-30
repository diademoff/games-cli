using System;

namespace Games
{
    /// <summary>
    /// Интерактивный - значит может обрабатывать нажатия клавиш.
    /// </summary>
    public interface IInteractive
    {
        /// <summary>
        /// Включена ли обработка нажатий.
        /// </summary>
        bool IsFocused { get; set; }
        /// <summary>
        /// Обработать нажатие.
        /// </summary>
        void HandleKey(ConsoleKey key);
    }
}