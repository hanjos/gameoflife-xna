using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        TimeSpan Tick { get; }
        bool RunAtStart { get; }

        Either<Keys, MouseButtons> ToggleCell { get; }
        Either<Keys, MouseButtons> ToggleRunning { get; }
        Either<Keys, MouseButtons> SpeedUp { get; }
        Either<Keys, MouseButtons> SlowDown { get; }
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
            Rows = config.Rows;
            Columns = config.Columns;
            Tick = TimeSpan.FromMilliseconds(config.TickInMilliseconds);
            RunAtStart = config.RunAtStart;

            ToggleCell = ExtractCommand(config.Commands.ToggleCell);
            ToggleRunning = ExtractCommand(config.Commands.ToggleRunning);
            SpeedUp = ExtractCommand(config.Commands.SpeedUp);
            SlowDown = ExtractCommand(config.Commands.SlowDown);
            Quit = ExtractCommand(config.Commands.Quit);
        }

        protected Either<Keys, MouseButtons> ExtractCommand(string input)
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
