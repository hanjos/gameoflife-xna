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
    #region Constants
    public static class Constants
    {
        public const string CONFIG_FILE = "config";
    }
    #endregion

    #region Main Game
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

            state = new State(this);
            Components.Add(state);

            view = new View(this);
            Components.Add(view);

            // rest
            IsMouseVisible = true;
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // setting up the input
            input.Register(
                MouseButtons.LeftButton,
                (current, gameTime) =>
                {
                    state.World.Toggle(view.XToRow(current.X), view.YToColumn(current.Y));
                });

            input.Register(
                Keys.Space,
                (current, gameTime) =>
                {
                    state.ToggleRunning();
                });

            input.Register(
                Keys.Up,
                (current, gameTime) =>
                {
                    state.DecreaseTick();
                });

            input.Register(
                Keys.Down,
                (current, gameTime) =>
                {
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
        private State state;
        private View view;
        #endregion
    }
    #endregion
}
