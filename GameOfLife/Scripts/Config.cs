namespace Scripts
{
    public struct Config
    {
        public int Rows;
        public int Columns;
        public int TickInMilliseconds;
        public bool RunAtStart;
        public Grid Grid;
        public Commands Commands;
    }

    public struct Grid
    {
        public bool DrawAtStart;
        public string Color;
    }

    public struct Commands
    {
        public string ToggleCell;
        public string ToggleRunning;
        public string ToggleGrid;
        public string SpeedUp;
        public string SlowDown;
        public string Clear;
        public string Quit;
    }
}
