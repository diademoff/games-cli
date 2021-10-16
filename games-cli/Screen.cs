using System;
using System.Drawing;
using System.Threading.Tasks;

namespace Games
{
    abstract class Screen : IInteractive, IDrawableElement
    {
        public abstract void HandleKey(ConsoleKey key);
        public Action<object> OnExit { get; set; }
        public bool Exited { get; protected set; } = false;
        protected void Exit(object output)
        {
            Exited = true;
            OnExit?.Invoke(output);
        }
        public bool IsFocused { get => true; set => throw new Exception("Screen is always focused"); }
        public abstract IDrawable[] ElementContent { get; }
        public abstract void OnWindowSizeChanged(Size fieldSize);
        public async Task WaitForExit()
        {
            do
            {
                await Task.Delay(100);
            } while (!Exited);
        }
    }
}