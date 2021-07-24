using System;
using System.Threading;

namespace Games
{
    class ScreenCaller
    {
        object prevScreenResult;
        private ScreenCaller() { }

        public static ScreenCaller Call(Screen screen, Drawer d, int delay)
        {
            var output = new ScreenCaller();

            screen.OnExit += (o) => { output.prevScreenResult = o; };

            do
            {
                d.Create(screen);
                d.DrawToConsole();

                Thread.Sleep(delay);
            } while (!screen.Exited);

            d.Remove(screen);
            d.DrawToConsole();

            screen.OnExit -= (o) => { output.prevScreenResult = o; };

            return output;
        }

        public void OnExit(Action<object> action)
        {
            action.Invoke(prevScreenResult);
        }
    }
}