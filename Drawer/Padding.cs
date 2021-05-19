/*
Класс задет отступы, которые можно передать в качестве аргумента
*/
struct Padding
{
    // Отступ слева
    public int Left { get; set; }
    // Отступ справа
    public int Right { get; set; }
    // Отступ сверху
    public int Top { get; set; }
    // Отступ снизу
    public int Buttom { get; set; }

    public Padding(int left, int right, int top, int buttom)
    {
        Left = left;
        Right = right;
        Top = top;
        Buttom = buttom;
    }
}