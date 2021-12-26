using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Games
{
    public class FlappyBirdGame : Game
    {
        public override bool IsFocused { get; set; } = true;

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
        TextField scoreCounter => new TextField(new Point(2, FieldSize.Height - Padding.Bottom + 2), 15, $"Score: {score}");

        const int columnMoveOffset = 2;
        const int birdFallOffset = 1;
        const int birdJumpOffset = 7;
        const int columnWidth = 5;
        int blockInterval;
        public FlappyBirdGame(Size FieldSize, Padding p) : base(FieldSize, p)
        {
            Init();
        }

        void Init()
        {
            int birdInitLocation_Y = (FieldSize.Height - Padding.Top - Padding.Bottom) / 2;
            this.bird = new Bird(jumpSize: birdJumpOffset, new Point(20, birdInitLocation_Y));
            this.buttonLine = new Line('-', new Point(Padding.Left, FieldSize.Height - Padding.Bottom), new Point(FieldSize.Width - Padding.Right, FieldSize.Height - Padding.Bottom));
            this.topLine = new Line('-', new Point(Padding.Left, Padding.Top), new Point(FieldSize.Width - Padding.Right, Padding.Top));
            this.drawnColumns = new List<IEnumerable<IDrawable>>();
            this.score = 0;
            this.GameOverActionMenu = GetDefaultGameOverMenu();
            this.GameOverActionMenu.IsFocused = false;
            this.MenuPaused = GetDefaultPauseMenu();
            this.MenuPaused.IsFocused = false;

            InitColumns();
        }

        void InitColumns()
        {
            // Чтобы птичка успела долететь
            this.blockInterval = FieldSize.Height / birdFallOffset;
            // Чтобы весь экран был заполнен кононнами
            int colСount = (FieldSize.Width / blockInterval) + 3;
            this.columns = new Column[colСount];
            for (int i = 0; i < colСount; i++)
            {
                int n = i + 1; // number in order
                columns[i] = GetColumn(FieldSize.Width + (blockInterval * n));
            }
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (GameOverActionMenu.IsFocused)
            {
                GameOverActionMenu.HandleKey(key);
                return;
            }

            if (key == ConsoleKey.Escape)
            {
                MenuPaused.IsFocused = !MenuPaused.IsFocused;
            }

            if (MenuPaused.IsFocused)
            {
                MenuPaused.HandleKey(key);
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
            if (MenuPaused.IsFocused)
            {
                d.Create(MenuPaused);
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
            if (!MenuPaused.IsSelected)
            {
                return;
            }

            if (MenuPaused.SelectedIndex == 0)
            {
                MenuPaused.IsFocused = false;
            }
            else if (MenuPaused.SelectedIndex == 1)
            {
                RemoveGame(d);
                isGameOver = true;
            }
            MenuPaused.Reuse();
        }

        /**
        Пользователь выбирает действие
        */
        void GameOverUserAction(Drawer d)
        {
            d.Create(GameOverActionMenu);
            this.GameOverActionMenu.IsFocused = true;
            if (!GameOverActionMenu.IsSelected)
            {
                return;
            }

            RemoveGame(d);
            d.Remove(GameOverActionMenu);

            if (GameOverActionMenu.SelectedIndex == 0)
            {
                Init();
            }
            else if (GameOverActionMenu.SelectedIndex == 1)
            {
                isGameOver = true;
            }
        }

        bool IsBirdIntersectsWithColumn(IEnumerable<IDrawable> birdContent)
        {
            foreach (var birdPixel in birdContent)
            {
                foreach (var column in columns)
                {
                    var colContent = column.ElementContent;
                    foreach (var colPixel in colContent)
                    {
                        if (colPixel.Location == birdPixel.Location)
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
                var contentCuttted = CutContentVertical(content);
                d.Create(contentCuttted);
                drawnColumns.Add(contentCuttted);

                if (IsGoneLeft(content))
                {
                    // Получить координаты самой правой колонки
                    int rightCol = columns.OrderBy(x => x.ElementContent.OrderBy(y => y.Location.X).Last().Location.X).Last().ElementContent.OrderBy(z => z.Location.X).Last().Location.X;
                    columns[i] = GetColumn(rightCol + blockInterval);
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
                if (c.Location.X >= Padding.Left)
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
                if (c.Location.X < Padding.Left ||
                    c.Location.X > FieldSize.Width - Padding.Right)
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
            return new Column(verticalLocation, intervalHeight: random.Next(13, 15), columnWidth, FieldSize, random, Padding);
        }

        bool BirdIntersectsBorder(IDrawable[] birdContent)
        {
            foreach (IDrawable i in birdContent)
            {
                if (i.Location.Y <= Padding.Top ||
                    i.Location.Y >= FieldSize.Height - Padding.Bottom)
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
            d.Remove(MenuPaused);
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            RemoveGame(d);
        }
    }
}