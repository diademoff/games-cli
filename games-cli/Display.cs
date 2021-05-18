/*
Управление содержимым экрана.
*/

using System;

class Display : IInteractive
{
    /*
    Находится ли голова змейки на яблоке.
    */
    public bool IsAppleEaten => snake.IsEaten(apple);

    /*
    столкнулась ли змейка с собой или с краем.
    */
    public bool IsSnakeIntersect => snake.SelfIntersect() || snake.BorderIntersect(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

    /*
    Ускорение
    */
    bool speedUp = false;
    /*
    Высчитать задержку между кадрами исходя из текущего прогресса и
    ускорения.
    */
    public int FrameDelay => frameDelay();
    int frameDelay()
    {
        if (speedUp)
        {
            speedUp = false;
            return progress.Delay / 4;
        }
        return progress.Delay;
    }
    int FIELD_SIZE_WIDTH;
    int FIELD_SIZE_HEIGHT;
    int INIT_DELAY = 100;

    Snake snake;
    /*
    Для отрисовки используется класс Drawer. Он предоставляет
    возможность отрисовать IDrawable.
    */
    Drawer drawer;
    Apple apple;
    /*
    Прогресс игрока
    */
    Progress progress;
    Random rnd = new Random();

    Padding p = new Padding(1, 1, 3, 5);
    MessageBox info_paused;

    public Display()
    {
        FIELD_SIZE_WIDTH = Console.WindowWidth;
        FIELD_SIZE_HEIGHT = Console.WindowHeight;

        drawer = new Drawer(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT);
        progress = new Progress(INIT_DELAY, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 1);

        Console.Title = "snake-cli";
        Console.CursorVisible = false;

        info_paused = new MessageBox("Press ESC to resume", 30, 5,
                                                    FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

        snake = new Snake('*', p);
        drawer.CreateBorder('·', p);

        RegenerateApple(p);

        drawer.RedrawAll();
    }

    // Отрисовать окно Game over
    public void GameOver()
    {
        MessageBox box = new MessageBox("GAME OVER", 50, 7,
                    FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p);

        drawer.Create(box);
        Flush();
        drawer.RedrawAll();

        Console.CursorVisible = true;
    }

    // Отрисовать окна паузы
    public void Paused()
    {
        drawer.Create(info_paused);
        Flush();
    }

    /*
    Удовлетворить запросы на отрисовку
    */
    public void Flush()
    {
        drawer.DrawToConsole();
    }

    public void MoveSnake()
    {
        UnPause(); // стереть надпись паузы
        RemoveSnake(); // Удалить старую змейку

        snake.Move();

        if (IsAppleEaten)
        {
            AddBlock();
        }

        Draw(); // добавить в очередь на отрисовку всё содержимое

        Flush(); // отрисовать очередь
    }

    public bool IsFocused { get => true; set => throw new NotImplementedException(); }

    public void HandleKey(ConsoleKey key)
    {
        if (key == ConsoleKey.Spacebar)
        {
            speedUp = true;
            return;
        }
        snake.HandleKey(key);
    }

    /*
    Добавить в очередь запросы на отрисовку компонентов
    */
    void Draw()
    {
        drawer.Create(snake);
        drawer.Create(apple);
        drawer.Create(progress.StatusBar);
    }

    void AddBlock()
    {
        snake.AddBlock();
        drawer.Remove(apple); // удалить старое яблоко
        RegenerateApple(p);
        progress.AppleEaten();
    }

    // Запрос на удаление окна паузы
    void UnPause()
    {
        drawer.Remove(info_paused);
    }

    // Запрос на удаление змейки
    void RemoveSnake()
    {
        drawer.Remove(snake); // запрос на удаление змейки
    }

    void RegenerateApple(Padding p)
    {
        apple = new Apple(new AppleGen(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, snake, p), ref rnd);
    }
}