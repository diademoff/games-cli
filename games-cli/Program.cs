using System;
using System.Threading;
using System.Threading.Tasks;

namespace Games
{
    class Program
    {
        static Display display;
        static void Main(string[] args)
        {
            display = new Display();

            display.StartScreen();

            Console.CursorVisible = true;
        }
    }
}
