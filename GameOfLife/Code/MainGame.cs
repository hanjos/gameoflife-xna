using GameOfLife.Graphics;
using GameOfLife.Input;
using GameOfLife.GameState;
using GameOfLife.Settings;
using Microsoft.Xna.Framework.Input;

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

            keyInput = new KeyInput(this);
            Components.Add(keyInput);

            mouseInput = new MouseInput(this);
            Components.Add(mouseInput);

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
            mouseInput.Register(
                settings.ToggleCell.As<MouseButtons>(),
                (current, gameTime) =>
                {
                    state.World.Toggle(view.XToRow(current.X), view.YToColumn(current.Y));
                });

            keyInput.Register(
                settings.ToggleRunning.As<Keys>(),
                (current, gameTime) =>
                {
                    state.ToggleRunning();
                });

            keyInput.Register(
                settings.ToggleGrid.As<Keys>(),
                (current, gameTime) =>
                {
                    view.DrawGrid = !view.DrawGrid;
                });

            keyInput.Register(
                settings.SpeedUp.As<Keys>(),
                (current, gameTime) =>
                {
                    state.DecreaseTick();
                });

            keyInput.Register(
                settings.SlowDown.As<Keys>(),
                (current, gameTime) =>
                {
                    state.IncreaseTick();
                });

            keyInput.Register(
                settings.Clear.As<Keys>(),
                (current, gameTime) =>
                {
                    state.Clear();
                });

            keyInput.Register(
                settings.Quit.As<Keys>(),
                (current, gameTime) =>
                {
                    Exit();
                });
        }
        #endregion

        #region Fields
        private DefaultSettings settings;
        private KeyInput keyInput;
        private MouseInput mouseInput;
        private State state;
        private View view;
        #endregion
    }
    #endregion
}
