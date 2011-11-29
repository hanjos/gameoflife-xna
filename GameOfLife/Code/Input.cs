using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameOfLife.Graphics;
using GameOfLife.GameState;

namespace GameOfLife.Input
{
    #region Input Component
    public interface IInput
    {
        
    }

    public class InputManager : Microsoft.Xna.Framework.GameComponent, IInput
    {
        public InputManager(Game game) : base(game)
        {
            // registering itself as a service
            game.Services.AddService(typeof(IInput), this);
        }

        #region Operations
        public override void Update(GameTime gameTime)
        {
            CheckMouseEvents(gameTime);
            CheckKeyboardEvents(gameTime);
        }
        #endregion

        #region Event Detection & Raising
        protected void CheckMouseEvents(GameTime gameTime)
        {
            MouseState current = Mouse.GetState();

            if (DetectMouseClicked(MouseButtons.LeftButton)(LastMouseState, current, gameTime))
            {
                IView view = (IView) Game.Services.GetService(typeof(IView));
                IState state = (IState) Game.Services.GetService(typeof(IState));

                state.World.Toggle(view.XToRow(current.X), view.YToColumn(current.Y));
            }

            LastMouseState = current;
        }

        protected void CheckKeyboardEvents(GameTime gameTime)
        {
            KeyboardState current = Keyboard.GetState();

            if (DetectKeyPressed(Keys.Space)(LastKeyboardState, current, gameTime))
            {
                IState state = (IState) Game.Services.GetService(typeof(IState));
                state.ToggleRunning();
            }

            if (DetectKeyPressed(Keys.Up)(LastKeyboardState, current, gameTime))
            {
                IState state = (IState) Game.Services.GetService(typeof(IState));
                state.DecreaseTick();
            }

            if (DetectKeyPressed(Keys.Down)(LastKeyboardState, current, gameTime))
            {
                IState state = (IState)Game.Services.GetService(typeof(IState));
                state.IncreaseTick();
            }

            if (DetectKeyPressed(Keys.Escape)(LastKeyboardState, current, gameTime))
            {
                Game.Exit();
            }

            LastKeyboardState = current;
        }

        protected Func<KeyboardState, KeyboardState, GameTime, bool> DetectKeyPressed(Keys key)
        {
            return (KeyboardState last, KeyboardState current, GameTime gameTime) => last.IsKeyDown(key) && current.IsKeyUp(key);
        }

        protected Func<MouseState, MouseState, GameTime, bool> DetectMouseClicked(MouseButtons key)
        {
            return key.DetectClick;
        }
        #endregion

        #region Properties & Fields
        private MouseState LastMouseState
        {
            get { if (lastMouseState_ == null) lastMouseState_ = Mouse.GetState(); return lastMouseState_; }
            set { lastMouseState_ = value; }
        }

        private KeyboardState LastKeyboardState
        {
            get { if (lastKeyboardState_ == null) lastKeyboardState_ = Keyboard.GetState(); return lastKeyboardState_; }
            set { lastKeyboardState_ = value; }
        }

        private MouseState lastMouseState_;
        private KeyboardState lastKeyboardState_;
        #endregion
    }
    #endregion

    #region Mouse Button Abstraction
    public abstract class MouseButtons
    {
        public static readonly MouseButtons LeftButton = new LeftButtonMouseButtons();
        public static readonly MouseButtons RightButton = new RightButtonMouseButtons();
        public static readonly MouseButtons MiddleButton = new MiddleButtonMouseButtons();

        public abstract bool DetectClick(MouseState last, MouseState current, GameTime gameTime);

        private class LeftButtonMouseButtons : MouseButtons
        {
            public override bool DetectClick(MouseState last, MouseState current, GameTime gameTime)
            {
                return last.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released;
            }
        }

        private class RightButtonMouseButtons : MouseButtons
        {
            public override bool DetectClick(MouseState last, MouseState current, GameTime gameTime)
            {
                return last.RightButton == ButtonState.Pressed && current.RightButton == ButtonState.Released;
            }
        }

        private class MiddleButtonMouseButtons : MouseButtons
        {
            public override bool DetectClick(MouseState last, MouseState current, GameTime gameTime)
            {
                return last.MiddleButton == ButtonState.Pressed && current.MiddleButton == ButtonState.Released;
            }
        }
    }
    #endregion

    #region Event Args
    public class InputArgs<T> : EventArgs
    {
        public InputArgs(T last, T current, GameTime gameTime)
        {
            Last = last;
            Current = current;
            GameTime = gameTime;
        }

        #region Properties & Fields
        private T last;
        private T current;
        private GameTime gameTime;

        public T Last
        {
            get { return last; }
            private set { last = value; }
        }

        public T Current
        {
            get { return current; }
            private set { current = value; }
        }

        public GameTime GameTime
        {
            get { return gameTime; }
            private set { gameTime = value; }
        }
        #endregion
    }
    #endregion
}

