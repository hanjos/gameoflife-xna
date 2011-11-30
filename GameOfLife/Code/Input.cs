using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameOfLife.Graphics;
using GameOfLife.GameState;

namespace GameOfLife.Input
{
    #region Input Component
    public interface IInput
    {
        void Register(Keys key, Action<KeyboardState, GameTime> action);
        void Register(MouseButtons mouseButton, Action<MouseState, GameTime> action);
    }

    public class InputManager : Microsoft.Xna.Framework.GameComponent, IInput
    {
        public InputManager(Game game) : base(game)
        {
            _keyboardInput = new Dictionary<Keys, Action<KeyboardState, GameTime>>();
            _mouseInput = new Dictionary<MouseButtons, Action<MouseState, GameTime>>();

            // registering itself as a service
            game.Services.AddService(typeof(IInput), this);
        }

        #region Operations
        public override void Update(GameTime gameTime)
        {
            CheckMouseEvents(gameTime);
            CheckKeyboardEvents(gameTime);
        }

        public virtual void Register(Keys key, Action<KeyboardState, GameTime> action)
        {
            _keyboardInput[key] = action;
        }

        public virtual void Register(MouseButtons mouseButton, Action<MouseState, GameTime> action)
        {
            _mouseInput[mouseButton] = action;
        }
        #endregion

        #region Event Detection & Raising
        protected void CheckMouseEvents(GameTime gameTime)
        {
            MouseState current = Mouse.GetState();

            foreach (var entry in _mouseInput)
            {
                if (DetectMouseClicked(entry.Key)(LastMouseState, current, gameTime))
                {
                    entry.Value(current, gameTime);
                }
            }

            LastMouseState = current;
        }

        protected void CheckKeyboardEvents(GameTime gameTime)
        {
            KeyboardState current = Keyboard.GetState();

            foreach (var entry in _keyboardInput)
            {
                if (DetectKeyPressed(entry.Key)(LastKeyboardState, current, gameTime))
                {
                    entry.Value(current, gameTime);
                }
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
            get { if (_lastMouseState == null) _lastMouseState = Mouse.GetState(); return _lastMouseState; }
            set { _lastMouseState = value; }
        }
        private MouseState _lastMouseState;
        
        private KeyboardState LastKeyboardState
        {
            get { if (_lastKeyboardState == null) _lastKeyboardState = Keyboard.GetState(); return _lastKeyboardState; }
            set { _lastKeyboardState = value; }
        }
        private KeyboardState _lastKeyboardState;

        private IDictionary<MouseButtons, Action<MouseState, GameTime>> _mouseInput;
        private IDictionary<Keys, Action<KeyboardState, GameTime>> _keyboardInput;
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

