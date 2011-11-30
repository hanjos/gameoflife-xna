using Microsoft.Xna.Framework;
using GameOfLife.Graphics;
using GameOfLife.Input;
using GameOfLife.Model;
using GameOfLife.GameState;
using GameOfLife.Settings;
using Microsoft.Xna.Framework.Input;
using Scripts;
using System;

namespace GameOfLife
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        #region Initialization
        public MainGame()
        {
            // game components
            settings = new DefaultSettings(this);
            Components.Add(settings);

            input = new InputManager(this);
            Components.Add(input);

            Components.Add(new State(this));
            Components.Add(new View(this));

            // rest
            IsMouseVisible = true;
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // extracting config.xml's data, and making it available via Settings
            Config config = Content.Load<Config>("config");

            settings.LoadFrom(config);

            // setting up the input
            input.Register(
                MouseButtons.LeftButton,
                (current, gameTime) =>
                {
                    IView view = (IView) Services.GetService(typeof(IView));
                    IState state = (IState) Services.GetService(typeof(IState));

                    state.World.Toggle(view.XToRow(current.X), view.YToColumn(current.Y));
                });

            input.Register(
                Keys.Space,
                (current, gameTime) =>
                {
                    IState state = (IState) Services.GetService(typeof(IState));
                    state.ToggleRunning();
                });

            input.Register(
                Keys.Up,
                (current, gameTime) =>
                {
                    IState state = (IState) Services.GetService(typeof(IState));
                    state.DecreaseTick();
                });

            input.Register(
                Keys.Down,
                (current, gameTime) =>
                {
                    IState state = (IState) Services.GetService(typeof(IState));
                    state.IncreaseTick();
                });

            input.Register(
                Keys.Escape,
                (current, gameTime) =>
                {
                    Exit();
                });

            // now we can initialize everybody
            base.Initialize();
        }
        #endregion

        #region Fields
        private DefaultSettings settings;
        private InputManager input;
        #endregion
    }
}
