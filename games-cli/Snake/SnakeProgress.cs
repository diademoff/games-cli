using System.Drawing;

namespace Games
{
    /**
    Класс для отслеживания прогресса игры в змейку. Состоит
    только из текстового поля, в котором написаны очки
    */
    public class SnakeProgress
    {
        public TextField StatusBar { get; }
        public int Delay { get; set; }
        public int Score { get; set; }

        public SnakeProgress(int delay, Size fieldSize, int buttonShift)
        {
            this.Score = 0;
            this.Delay = delay;

            this.StatusBar = new TextField(new System.Drawing.Point(0, fieldSize.Height - buttonShift),
                                fieldSize.Width);
            ChangeStatusBarText();
        }

        public void AppleEaten()
        {
            Score += 1;
            ChangeStatusBarText();
            if (Delay > 50)
            {
                Delay -= 5;
            }
        }

        private void ChangeStatusBarText()
        {
            StatusBar.Text = $"Score: {Score}";
        }
    }
}