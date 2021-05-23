namespace Games
{
    /**
    Элемент, который можно вывести в консоль
    */
    public interface IDrawableElement
    {
        /**
        Любой элемент это просто массив из символов.
        */
        IDrawable[] ElementContent { get; }
    }
}