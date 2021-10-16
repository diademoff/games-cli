namespace Games
{
    /**
    Класс задет отступы которые можно передать в качестве аргумента
    */
    public struct Padding
    {
        /// Отступ слева
        public int Left { get; set; }
        /// Отступ справа
        public int Right { get; set; }
        /// Отступ сверху
        public int Top { get; set; }
        /// Отступ снизу
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