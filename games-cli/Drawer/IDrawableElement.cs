/*
Элемент, который можно вывести в консоль
*/

namespace Games
{
    public interface IDrawableElement
    {
        /*
        Любой элемент это просто массив из символов.
        Here it is.
        */
        IDrawable[] ElementContent { get; }
    }
}