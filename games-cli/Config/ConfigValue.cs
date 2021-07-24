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

    public class ConfigCharValue : IConfigValue<char>
    {
        public char Value { get; set; }
        public void ChangeValueTo(string v) => this.Value = v[0];
        public ConfigCharValue(char c) => this.Value = c;
    }

    public class ConfigIntValue : IConfigValue<int>
    {
        public int Value { get; set; }
        public void ChangeValueTo(string v) => this.Value = int.Parse(v);
        public ConfigIntValue(int n) => this.Value = n;
    }
}