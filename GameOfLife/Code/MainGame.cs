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
            // first initialize everybody
            base.Initialize();

            // now set the input
            input.Register(
                settings.ToggleCell.As<MouseButtons>(),
                (current, gameTime) =>
                {
                    state.World.Toggle(view.XToRow(current.X), view.YToColumn(current.Y));
                });

            input.Register(
                settings.ToggleRunning.As<Keys>(),
                (current, gameTime) =>
                {
                    state.ToggleRunning();
                });

            input.Register(
                settings.ToggleGridLines.As<Keys>(),
                (current, gameTime) =>
                {
                    view.DrawGridLines = !view.DrawGridLines;
                });

            input.Register(
                settings.SpeedUp.As<Keys>(),
                (current, gameTime) =>
                {
                    state.DecreaseTick();
                });

            input.Register(
                settings.SlowDown.As<Keys>(),
                (current, gameTime) =>
                {
                    state.IncreaseTick();
                });

            input.Register(
                settings.Quit.As<Keys>(),
                (current, gameTime) =>
                {
                    Exit();
                });
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
