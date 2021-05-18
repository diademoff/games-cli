/*
Класс для отслеживания прогресса игрока
*/

namespace Games
{
    public class Progress
    {
        public TextField StatusBar { get; }
        public int Delay { get; set; }
        public int Score { get; set; }

        public Progress(int delay, int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, int buttonShift)
        {
            this.Score = 0;
            this.Delay = delay;

            this.StatusBar = new TextField(new System.Drawing.Point(0, FIELD_SIZE_HEIGHT - buttonShift),
                                FIELD_SIZE_WIDTH);
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