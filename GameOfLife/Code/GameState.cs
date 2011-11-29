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

namespace GameOfLife.GameState
{
    #region Game State
    public interface IState
    {
        bool Running { get; set; }
        World World { get; set; }
        TimeSpan Tick { get; set; }

        event EventHandler<RunningToggled> RunningToggled;
    }

    public class State : Microsoft.Xna.Framework.GameComponent, IState
    {
        public State(Game game, World world) : base(game)
        {
            World = world;
            Running = false;
            _timeOfLastTick = TimeSpan.Zero;
            Tick = TimeSpan.FromMilliseconds(100);

            // registering itself as a service
            game.Services.AddService(typeof(IState), this);
        }

        public override void Initialize()
        {
            IInput input = (IInput) Game.Services.GetService(typeof(IInput));
            input.ExecutionToggle +=
                (sender, args) =>
                {
                    _timeOfLastTick = args.GameTime.TotalGameTime;
                    Running = !Running;
                };
            input.QuitGame += (sender, args) => Game.Exit();
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Running && gameTime.TotalGameTime - _timeOfLastTick > Tick)
            {
                World.Tick();
                _timeOfLastTick = gameTime.TotalGameTime;
            }

            base.Update(gameTime);
        }

        #region Events
        public event EventHandler<RunningToggled> RunningToggled;

        // for derived classes to use
        protected virtual void RaiseRunningToggled(RunningToggled args)
        {
            if (RunningToggled != null)
                RunningToggled(this, args);
        }
        #endregion

        #region Properties & Fields
        public bool Running
        {
            get { return _running; }
            set { _running = value; RaiseRunningToggled(new RunningToggled(value)); }
        }
        private bool _running;

        public World World
        {
            get { return _world; }
            set { _world = value; }
        }
        private World _world;

        public TimeSpan Tick 
        {
            get { return _tick; }
            set { _tick = value; }
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

        #region Properties & Fields
        public bool Current
        {
            get { return _current; }
            private set { _current = value; }
        }

        private bool _current;
        #endregion
    }
    #endregion
}
