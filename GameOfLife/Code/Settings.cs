using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Scripts;
using GameOfLife.Utilities;
using GameOfLife.Input;

namespace GameOfLife.Settings
{
    #region Exceptions
    public class SettingsException : ApplicationException
    {
        public SettingsException() { }

        public SettingsException(string message) : base(message) { }

        public SettingsException(string message, Exception inner) : base(message, inner) { }
    }

    public class UnknownCommandException : SettingsException
    {
        public UnknownCommandException(string command) : base("Unknown command: " + command)
        {
            Command = command;
        }

        public UnknownCommandException(string command, Exception inner) : base("Unknown command: " + command, inner)
        {
            Command = command;
        }

        public string Command
        {
            get { return _command; }
            private set { _command = value; }
        }
        private string _command;
    }
    #endregion

    #region Settings Component
    public interface ISettings
    {
        int Rows { get; }
        int Columns { get; }
        int CellWidth { get; }
        int CellHeight { get; }
        Color DeadColor { get; }
        Color RunningColor { get; }
        Color LiveColor { get; }

        TimeSpan Tick { get; }
        bool RunAtStart { get; }

        bool DrawGridAtStart { get; }
        Color GridColor { get; }

        Either<Keys, MouseButtons> ToggleCell { get; }
        Either<Keys, MouseButtons> ToggleRunning { get; }
        Either<Keys, MouseButtons> ToggleGrid { get; }
        Either<Keys, MouseButtons> SpeedUp { get; }
        Either<Keys, MouseButtons> SlowDown { get; }
        Either<Keys, MouseButtons> Clear { get; }
        Either<Keys, MouseButtons> Quit { get; }
    }

    public class DefaultSettings : Microsoft.Xna.Framework.GameComponent, ISettings
    {
        #region Initialization
        public DefaultSettings(Game game) : base(game)
        {
            // registering the services
            game.Services.AddService(typeof(ISettings), this);
        }

        public override void Initialize()
        {
            // extracting config.xml's data, and making it available via Settings
            LoadFrom(Game.Content.Load<Config>(Constants.CONFIG_FILE));

            base.Initialize();
        }
        #endregion

        #region Operations
        protected void LoadFrom(Config config)
        {
            //worldly concerns
            Rows = config.World.Rows;
            Columns = config.World.Columns;
            CellWidth = config.World.CellWidth;
            CellHeight = config.World.CellHeight;
            DeadColor = ExtractColorFrom(config.World.DeadColor);
            RunningColor = ExtractColorFrom(config.World.RunningColor);
            LiveColor = ExtractColorFrom(config.World.LiveColor);

            // time troubles
            Tick = TimeSpan.FromMilliseconds(config.TickInMilliseconds);
            RunAtStart = config.RunAtStart;

            // grid issues
            DrawGridAtStart = config.Grid.DrawAtStart;
            GridColor = ExtractColorFrom(config.Grid.Color);

            // commands
            ToggleCell = ExtractCommandFrom(config.Commands.ToggleCell);
            ToggleRunning = ExtractCommandFrom(config.Commands.ToggleRunning);
            ToggleGrid = ExtractCommandFrom(config.Commands.ToggleGrid);
            SpeedUp = ExtractCommandFrom(config.Commands.SpeedUp);
            SlowDown = ExtractCommandFrom(config.Commands.SlowDown);
            Clear = ExtractCommandFrom(config.Commands.Clear);
            Quit = ExtractCommandFrom(config.Commands.Quit);
        }

        protected Either<Keys, MouseButtons> ExtractCommandFrom(string input)
        {
            // try to convert to a key
            Keys key;
            if (Enum.TryParse(input, true, out key))
                return new Either<Keys, MouseButtons>(key);

            // try to convert to a mouse button
            MouseButtons mouseButton = ExtractCommandAsMouseInput(input);
            if (mouseButton != null)
                return new Either<Keys, MouseButtons>(mouseButton);

            throw new UnknownCommandException(input);
        }

        protected MouseButtons ExtractCommandAsMouseInput(string input)
        {
            if (input == null)
                return null;

            string lowercaseInput = input.ToLower();
            switch (lowercaseInput)
            { 
                case "leftbutton":
                    return MouseButtons.LeftButton;
                case "rightbutton":
                    return MouseButtons.RightButton;
                case "middlebutton":
                    return MouseButtons.MiddleButton;
                default:
                    return null;
            }
        }

        protected Color ExtractColorFrom(string input)
        {
            System.Drawing.Color color = System.Drawing.Color.FromName(input);
            return new Color(color.R, color.G, color.B, color.A);
        }
        #endregion

        #region ISettings
        public int Rows
        {
            get { return _rows; }
            private set { _rows = value; }
        }
        private int _rows;

        public int Columns
        {
            get { return _columns; }
            private set { _columns = value; }
        }
        private int _columns;

        public int CellWidth
        {
            get { return _cellWidth; }
            private set { _cellWidth = value; }
        }
        private int _cellWidth;

        public int CellHeight
        {
            get { return _cellHeight; }
            private set { _cellHeight = value; }
        }
        private int _cellHeight;

        public Color DeadColor
        {
            get { return _deadColor; }
            private set { _deadColor = value; }
        }
        private Color _deadColor;

        public Color RunningColor
        {
            get { return _runningColor; }
            private set { _runningColor = value; }
        }
        private Color _runningColor;

        public Color LiveColor
        {
            get { return _liveColor; }
            private set { _liveColor = value; }
        }
        private Color _liveColor;

        public TimeSpan Tick
        {
            get { return _tick; }
            private set { _tick = value; }
        }
        private TimeSpan _tick;

        public bool RunAtStart
        {
            get { return _runAtStart; }
            private set { _runAtStart = value; }
        }
        private bool _runAtStart;

        public bool DrawGridAtStart
        {
            get { return _drawGrid; }
            private set { _drawGrid = value; }
        }
        private bool _drawGrid;

        public Color GridColor
        {
            get { return _gridColor; }
            private set { _gridColor = value; }
        }
        private Color _gridColor;

        public Either<Keys, MouseButtons> ToggleCell
        {
            get { return _toggleCell; }
            private set { _toggleCell = value; }
        }
        private Either<Keys, MouseButtons> _toggleCell;

        public Either<Keys, MouseButtons> ToggleRunning
        {
            get { return _toggleRunning; }
            private set { _toggleRunning = value; }
        }
        private Either<Keys, MouseButtons> _toggleRunning;

        public Either<Keys, MouseButtons> ToggleGrid
        {
            get { return _toggleGridLines; }
            private set { _toggleGridLines = value; }
        }
        private Either<Keys, MouseButtons> _toggleGridLines;

        public Either<Keys, MouseButtons> SpeedUp
        {
            get { return _speedUp; }
            private set { _speedUp = value; }
        }
        private Either<Keys, MouseButtons> _speedUp;

        public Either<Keys, MouseButtons> SlowDown
        {
            get { return _slowDown; }
            private set { _slowDown = value; }
        }
        private Either<Keys, MouseButtons> _slowDown;

        public Either<Keys, MouseButtons> Clear
        {
            get { return _clear; }
            private set { _clear = value; }
        }
        private Either<Keys, MouseButtons> _clear;

        public Either<Keys, MouseButtons> Quit
        {
            get { return _quit; }
            private set { _quit = value; }
        }
        private Either<Keys, MouseButtons> _quit;
        #endregion
    }
    #endregion
}
