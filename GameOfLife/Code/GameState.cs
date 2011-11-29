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


namespace GameOfLife
{

    public interface IGameState
    {
        bool Running { get; set; }
        World World { get; set; }
        TimeSpan Tick { get; set; }
    }

    public class GameState : Microsoft.Xna.Framework.GameComponent, IGameState
    {
        public GameState(Game game, World world) : base(game)
        {
            World = world;
            Running = false;
            _timeOfLastTick = TimeSpan.Zero;
            Tick = TimeSpan.FromMilliseconds(100);

            // registering itself as a service
            game.Services.AddService(typeof(IGameState), this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            IInput input = (IInput) Game.Services.GetService(typeof(IInput));
            input.ExecutionToggle +=
                (sender, args) =>
                {
                    Running = !Running;
                    _timeOfLastTick = args.GameTime.TotalGameTime;
                };
            input.QuitGame += (sender, args) => Game.Exit();
            
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Running && gameTime.TotalGameTime - _timeOfLastTick > Tick)
            {
                World.Tick();
                _timeOfLastTick = gameTime.TotalGameTime;
            }

            base.Update(gameTime);
        }

        #region Properties & Fields
        public bool Running
        {
            get { return _running; }
            set { _running = value; }
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
}
