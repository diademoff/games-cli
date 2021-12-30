namespace Games
{
    /// <summary>
    /// Элемент, который можно вывести в консоль.
    /// </summary>
    public interface IDrawableElement
    {
        /// <summary>
        /// Любой элемент это просто массив из символов.
        /// </summary>
        IDrawable[] ElementContent { get; }
    }
}