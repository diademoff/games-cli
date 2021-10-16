namespace Games
{
    /**
    Хранилище настроек приложения
    */
    public class ConfigStorage
    {
        public ConfigCharValue SnakeChar;
        public ConfigCharValue SnakeGameBorderChar;
        /// Ускорение змейки в миллисекундах
        public ConfigIntValue SnakeSpeedUp;
        public ConfigCharValue SnakeGameAppleChar;

        public ConfigCharValue TetrisBorderChar;
        public ConfigIntValue TetrisPlayGroundWidth;

        private ConfigStorage() { }

        static ConfigStorage()
        {
            // Default settings
            Current = new ConfigStorage()
            {
                SnakeChar = new ConfigCharValue('*'),
                SnakeGameBorderChar = new ConfigCharValue('·'),
                SnakeSpeedUp = new ConfigIntValue(25),
                SnakeGameAppleChar = new ConfigCharValue('☼'),

                TetrisBorderChar = new ConfigCharValue('·'),
                TetrisPlayGroundWidth = new ConfigIntValue(20)
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
                }).BindTo(this.SnakeSpeedUp),
                new ConfigParam("Символ яблока", new string[]{
                    "☼", "ȯ", "a"
                }).BindTo(this.SnakeGameAppleChar),

                new ConfigParam("Символ обводки в игре тетрис", new string[]{
                    "·", "*", "x", "ο"
                }).BindTo(this.TetrisBorderChar),
                new ConfigParam("Ширина игрового поля тетриса", new string[]{
                    "15", "20", "25", "30", "35", "40"
                }).BindTo(this.TetrisPlayGroundWidth)
            };
        }
    }
}