using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    public class FlappyBirdGame : Game
    {
        public override bool IsFocused { get => isFocused; set => isFocused = value; }
        bool isFocused = true;

        public override int DelayBetweenFrames => 100;

        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        Bird bird;
        /*
        Фактически отрисованная птичка, чтобы ее стереть, так как
        состояние птички изменяется из другого потока.
        */
        IDrawable[] drawnBird;
        Column[] columns;
        List<IEnumerable<IDrawable>> drawnColumns;
        Line buttonLine;
        Line topLine;
        int score = 0;
        TextField scoreCounter => new TextField(new Point(2, FIELD_SIZE_HEIGHT - padding.Bottom + 2), 15, $"Score: {score}");
        SelectionMenu gameOverMenu;
        SelectionMenu pauseMenu;

        const int columnMoveOffset = 2;
        const int birdFallOffset = 1;
        const int birdJumpOffset = 7;
        const int columnWidth = 5;
        int blockInterval;
        public FlappyBirdGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p) : base(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p)
        {
            Init();
        }

        void Init()
        {
            int birdInitLocation_Y = (FIELD_SIZE_HEIGHT - padding.Top - padding.Bottom) / 2;
            this.bird = new Bird(jumpSize: birdJumpOffset, new Point(20, birdInitLocation_Y));
            this.buttonLine = new Line('-', new Point(padding.Left, FIELD_SIZE_HEIGHT - padding.Bottom), new Point(FIELD_SIZE_WIDTH - padding.Right, FIELD_SIZE_HEIGHT - padding.Bottom));
            this.topLine = new Line('-', new Point(padding.Left, padding.Top), new Point(FIELD_SIZE_WIDTH - padding.Right, padding.Top));
            this.drawnColumns = new List<IEnumerable<IDrawable>>();
            this.score = 0;
            this.gameOverMenu = new SelectionMenu(new string[]{
                "Restart",
                "Exit"
            }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
            this.gameOverMenu.IsFocused = false;
            this.pauseMenu = new SelectionMenu(new string[]{
                "Resume",
                "Exit"
            }, FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, 0, padding);
            this.pauseMenu.IsFocused = false;

            InitColumns();
        }

        void InitColumns()
        {
            // Чтобы птичка успела долететь
            this.blockInterval = FIELD_SIZE_HEIGHT / birdFallOffset;
            // Чтобы весь экран был заполнен кононнами
            int col_count = (FIELD_SIZE_WIDTH / blockInterval) + 3;
            this.columns = new Column[col_count];
            for (int i = 0; i < col_count; i++)
            {
                int n = i + 1; // number in order
                columns[i] = GetColumn(FIELD_SIZE_WIDTH + (blockInterval * n));
            }
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (gameOverMenu.IsFocused)
            {
                gameOverMenu.HandleKey(key);
                return;
            }

            if (key == ConsoleKey.Escape)
            {
                pauseMenu.IsFocused = !pauseMenu.IsFocused;
            }

            if (pauseMenu.IsFocused)
            {
                pauseMenu.HandleKey(key);
                return;
            }

            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow ||
                key == ConsoleKey.K || key == ConsoleKey.Spacebar)
            {
                bird.Jump();
            }
        }

        public override void NextFrame(Drawer d)
        {
            if (pauseMenu.IsFocused)
            {
                d.Create(pauseMenu);
                PauseMenuAction(d);
                return;
            }
            d.Create(scoreCounter);

            var birdContent = bird.ElementContent;
            // Проверять перед отрисовкой, чтобы не было исключения
            if (IsBirdIntersectsWithColumn(birdContent) || BirdIntersectsBorder(birdContent))
            {
                GameOverUserAction(d);
                return;
            }

            drawnBird = birdContent;
            d.Create(buttonLine);
            d.Create(topLine);

            d.Create(drawnBird);

            bird.Fall(birdFallOffset);
            MoveColumns(d);
        }

        void PauseMenuAction(Drawer d)
        {
            if (!pauseMenu.IsSelected)
            {
                return;
            }

            if (pauseMenu.SelectedIndex == 0)
            {
                pauseMenu.IsFocused = false;
            }
            else if (pauseMenu.SelectedIndex == 1)
            {
                RemoveGame(d);
                isGameOver = true;
            }
            pauseMenu.Reuse();
        }

        /**
        Пользователь выбирает действие
        */
        void GameOverUserAction(Drawer d)
        {
            d.Create(gameOverMenu);
            this.gameOverMenu.IsFocused = true;
            if (!gameOverMenu.IsSelected)
            {
                return;
            }

            RemoveGame(d);
            d.Remove(gameOverMenu);

            if (gameOverMenu.SelectedIndex == 0)
            {
                Init();
            }
            else if (gameOverMenu.SelectedIndex == 1)
            {
                isGameOver = true;
            }
        }

        bool IsBirdIntersectsWithColumn(IEnumerable<IDrawable> birdContent)
        {
            foreach (var bird_pixel in birdContent)
            {
                foreach (var column in columns)
                {
                    var col_content = column.ElementContent;
                    foreach (var col_pixel in col_content)
                    {
                        if (col_pixel.Location == bird_pixel.Location)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        void MoveColumns(Drawer d)
        {
            drawnColumns.Clear();
            for (int i = 0; i < columns.Length; i++)
            {
                Column c = columns[i]; // current

                c.MoveLeft(columnMoveOffset);
                var content = c.ElementContent;
                var content_cut = CutContentVertical(content);
                d.Create(content_cut);
                drawnColumns.Add(content_cut);

                if (IsGoneLeft(content))
                {
                    // Получить координаты самой правой колонки
                    int right_col = columns.OrderBy(x => x.ElementContent.OrderBy(y => y.Location.X).Last().Location.X).Last().ElementContent.OrderBy(z => z.Location.X).Last().Location.X;
                    columns[i] = GetColumn(right_col + blockInterval);
                    score += 1;
                }
            }
        }


        /**
        Находится ли элемент полностью за левым краем
        */
        bool IsGoneLeft(IEnumerable<IDrawable> content)
        {
            foreach (var c in content)
            {
                if (c.Location.X >= padding.Left)
                {
                    return false;
                }
            }
            return true;
        }

        /**
        Обрезает пиксели, которые выходят за границы вертикально
        */
        List<IDrawable> CutContentVertical(IEnumerable<IDrawable> content)
        {
            List<IDrawable> result = new List<IDrawable>();
            foreach (var c in content)
            {
                if (c.Location.X < padding.Left ||
                    c.Location.X > FIELD_SIZE_WIDTH - padding.Right)
                {
                    continue;
                }
                result.Add(c);
            }
            return result;
        }

        Random random = new Random();
        Column GetColumn(int verticalLocation)
        {
            return new Column(verticalLocation, intervalHeight: random.Next(13, 15), columnWidth, new Size(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT), random, padding);
        }

        bool BirdIntersectsBorder(IDrawable[] birdContent)
        {
            foreach (IDrawable i in birdContent)
            {
                if (i.Location.Y <= padding.Top ||
                    i.Location.Y >= FIELD_SIZE_HEIGHT - padding.Bottom)
                {
                    return true;
                }
            }

            return false;
        }

        void RemoveGame(Drawer d)
        {
            if (drawnBird != null)
            {
                d.Remove(drawnBird);
            }

            foreach (var col in drawnColumns)
            {
                d.Remove(col);
            }

            d.Remove(buttonLine);
            d.Remove(topLine);
            d.Remove(scoreCounter);
            d.Remove(pauseMenu);
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            RemoveGame(d);
        }
    }
}