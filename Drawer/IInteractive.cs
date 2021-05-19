/*
Интерактивный - значит может обрабатывать нажатия клавиш
*/
using System;

interface IInteractive
{
    bool IsFocused {get; set; }
    void HandleKey(ConsoleKey key);
}