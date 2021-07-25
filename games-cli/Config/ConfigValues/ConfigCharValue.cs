namespace Games
{
    public class ConfigCharValue : IConfigValue<char>
    {
        public char Value { get; set; }
        public void ChangeValueTo(string v) => this.Value = v[0];
        public ConfigCharValue(char c) => this.Value = c;
    }
}
