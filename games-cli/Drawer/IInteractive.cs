/*
Интерактивный - значит может обрабатывать нажатия клавиш
*/
using System;

namespace Games
{
    public interface IInteractive
    {
        bool IsFocused { get; set; }
        void HandleKey(ConsoleKey key);
    }
}