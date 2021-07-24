namespace Games
{
    public class ConfigStorage
    {
        public ConfigCharValue SnakeChar;
        public ConfigCharValue SnakeGameBorderChar;
        /// Ускорение змейки в миллисекундах
        public ConfigIntValue SnakeSpeedUp;
        public ConfigCharValue SnakeGameAppleChar;

        private ConfigStorage() { }

        static ConfigStorage()
        {
            // Default settings
            Current = new ConfigStorage()
            {
                SnakeChar = new ConfigCharValue('*'),
                SnakeGameBorderChar = new ConfigCharValue('·'),
                SnakeSpeedUp = new ConfigIntValue(10),
                SnakeGameAppleChar = new ConfigCharValue('☼')
            };
        }

        public static ConfigStorage Current;

        public ConfigParam[] GetConfigParams()
        {
            return new ConfigParam[]
            {
                new ConfigParam("Символ змейки", new string[]{
                    "*", "+"
                }).BindTo(this.SnakeChar),
                new ConfigParam("Символ границы в игре змейка", new string[]{
                    "·", "*", "x", "ο"
                }).BindTo(this.SnakeGameBorderChar),
                new ConfigParam("Сила ускорения в змейке", new string[]{
                    "15", "20", "25", "30", "50"
                }).BindTo(this.SnakeSpeedUp).DefaultSelected(1),
                new ConfigParam("Символ яблока", new string[]{
                    "☼", "ȯ"
                }).BindTo(this.SnakeGameAppleChar)
            };
        }
    }
}