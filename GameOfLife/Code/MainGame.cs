﻿using Microsoft.Xna.Framework;
using GameOfLife.Graphics;
using GameOfLife.Input;
using GameOfLife.Model;
using GameOfLife.GameState;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public MainGame()
        {
            // game components
            Components.Add(new State(this, new World(25, 25)));
            Components.Add(new View(this));
            Components.Add(new InputManager(this));

            // rest
            IsMouseVisible = true;
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // setting up the input
            IInput input = (IInput) Services.GetService(typeof(IInput));
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

            base.Initialize();
        }
    }
}
