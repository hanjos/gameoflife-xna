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
using GameOfLife.Input;
using GameOfLife.Model;
using GameOfLife.Utilities;
using GameOfLife.Settings;

namespace GameOfLife.GameState
{
    #region Game State
    public interface IState
    {
        bool Running { get; }
        World World { get; }
        TimeSpan Tick { get; }

        void ToggleRunning();
        bool IncreaseTick();
        bool DecreaseTick();

        event EventHandler<RunningToggled> RunningToggled;
        event EventHandler<TickChanged> TickChanged;
    }

    public class State : Microsoft.Xna.Framework.GameComponent, IState
    {
        #region Constructors & Initialization
        public State(Game game) : base(game)
        {
            _timeOfLastTick = TimeSpan.Zero;
            
            // registering itself as a service
            game.Services.AddService(typeof(IState), this);
        }

        public override void Initialize()
        {
            ISettings settings = (ISettings) Game.Services.GetService(typeof(ISettings));

            World = new World(settings.Rows, settings.Columns);
            Tick = settings.Tick;
            Running = settings.RunAtStart;

            base.Initialize();
        }
        #endregion

        #region Operations
        public override void Update(GameTime gameTime)
        {
            if (Running && gameTime.TotalGameTime - _timeOfLastTick > Tick)
            {
                World.Tick();
                _timeOfLastTick = gameTime.TotalGameTime;
            }

            base.Update(gameTime);
        }

        public virtual void ToggleRunning()
        {
            Running = !Running;
        }

        public virtual bool IncreaseTick()
        {
            TimeSpan old = Tick;

            Tick = TimeSpanUtils.Min(Tick + TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(1000));

            return old != Tick;
        }

        public virtual bool DecreaseTick()
        {
            TimeSpan old = Tick;

            Tick = TimeSpanUtils.Max(Tick - TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(0));

            return old != Tick;
        }
        #endregion

        #region Events
        public event EventHandler<RunningToggled> RunningToggled;
        public event EventHandler<TickChanged> TickChanged;

        // for derived classes to use
        protected virtual void RaiseRunningToggled(RunningToggled args)
        {
            if (RunningToggled != null)
                RunningToggled(this, args);
        }

        protected virtual void RaiseTickChanged(TickChanged args)
        {
            if (TickChanged != null)
                TickChanged(this, args);
        }
        #endregion

        #region Properties & Fields
        public bool Running
        {
            get { return _running; }
            private set 
            {
                if (_running == value)
                    return;

                _running = value;

                RaiseRunningToggled(new RunningToggled(value)); 
            }
        }
        private bool _running;

        public World World
        {
            get { return _world; }
            private set { _world = value; }
        }
        private World _world;

        public TimeSpan Tick 
        {
            get { return _tick; }
            private set 
            {
                if (_tick == value)
                    return;

                _tick = value;

                RaiseTickChanged(new TickChanged(value));
            }
        }
        private TimeSpan _tick;

        private TimeSpan _timeOfLastTick; // only for inner usage
        #endregion
    }
    #endregion

    #region Event Args
    public class RunningToggled : EventArgs
    {
        public RunningToggled(bool current)
        { 
            Current = current; 
        }

        public bool Current
        {
            get { return _current; }
            private set { _current = value; }
        }

        private bool _current;
    }

    public class TickChanged : EventArgs
    {
        public TickChanged(TimeSpan current)
        {
            Current = current;
        }

        public TimeSpan Current
        {
            get { return _current; }
            private set { _current = value; }
        }
        private TimeSpan _current;
    }
    #endregion
}
