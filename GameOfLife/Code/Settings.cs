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

namespace GameOfLife.Settings
{
    public interface ISettings
    {
        int Rows { get; }
        int Columns { get; }
        TimeSpan Tick { get; }
        bool StartRunning { get; }
    }

    public class DefaultSettings : Microsoft.Xna.Framework.GameComponent, ISettings
    {
        public DefaultSettings(Game game) : base(game)
        {
            // registering the services
            game.Services.AddService(typeof(ISettings), this);
        }

        #region Operations
        public void LoadFrom(Config config)
        {
            Rows = config.Rows;
            Columns = config.Columns;
            Tick = TimeSpan.FromMilliseconds(config.TickInMilliseconds);
            StartRunning = config.StartRunning;
        }
        #endregion

        #region ISettings
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }
        private int _rows;

        public int Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }
        private int _columns;

        public TimeSpan Tick
        {
            get { return _tick; }
            set { _tick = value; }
        }
        private TimeSpan _tick;

        public bool StartRunning
        {
            get { return _startRunning; }
            set { _startRunning = value; }
        }
        private bool _startRunning;
        #endregion
    }
}
