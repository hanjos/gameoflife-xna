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
    #region Settings Component
    public interface ISettings
    {
        int Rows { get; }
        int Columns { get; }
        TimeSpan Tick { get; }
        bool RunAtStart { get; }
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
        #endregion
    }
    #endregion
}
