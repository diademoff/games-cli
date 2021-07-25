namespace Games
{
    /**
    Обертка значимого типа для изменения значения конфига по ссылке
    */
    public interface IConfigValue<T>
    {
        T Value { get; }
        void ChangeValueTo(string v);
    }
}