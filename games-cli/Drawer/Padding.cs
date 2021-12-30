namespace Games
{
    /// <summary>
    /// Класс задет отступы которые можно передать в качестве аргумента
    /// </summary>
    public struct Padding
    {
        /// <summary>
        /// Отступ слева
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// Отступ справа
        /// </summary>
        public int Right { get; set; }
        /// <summary>
        /// Отступ сверху
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// Отступ снизу
        /// </summary>
        public int Bottom { get; set; }

        public Padding(int left, int right, int top, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}