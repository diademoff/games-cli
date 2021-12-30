namespace Games
{
    /// <summary>
    /// Обертка значимого типа для изменения значения конфига по ссылке
    /// </summary>
    public interface IConfigValue<T>
    {
        T Value { get; }
        void ChangeValueTo(string v);
    }
}