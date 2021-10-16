using System;
using System.Threading;

namespace Games
{
    /**
    Обертка для удобного вызова экранов и обработки выхода из них
    */
    class ScreenCaller
    {
        object prevScreenResult;

        /**
        Единственный способ создать объект ScreenCaller это вызвать какой-либо
        экран
        */
        private ScreenCaller() { }

        public static ScreenCaller Call(Screen screen, Drawer d, int delay)
        {
            return Call(screen, d, () => delay);
        }

        public static ScreenCaller Call(Screen screen, Drawer d, Func<int> delay)
        {
            var output = new ScreenCaller();

            screen.OnExit += (o) => { output.prevScreenResult = o; };

            do
            {
                d.Create(screen);
                d.DrawToConsole();

                Thread.Sleep(delay());
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