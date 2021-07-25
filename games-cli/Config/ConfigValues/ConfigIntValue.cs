namespace Games
{
    public class ConfigIntValue : IConfigValue<int>
    {
        public int Value { get; set; }
        public void ChangeValueTo(string v) => this.Value = int.Parse(v);
        public ConfigIntValue(int n) => this.Value = n;
    }
}