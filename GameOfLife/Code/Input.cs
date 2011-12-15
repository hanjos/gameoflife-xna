using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameOfLife.Graphics;
using GameOfLife.GameState;

namespace GameOfLife.Input
{
    #region Keyboard Input component
    public interface IKeyInput
    {
        void Register(Keys key, Action<KeyboardState, GameTime> action);
        Action<KeyboardState, GameTime> Unregister(Keys key);
    }

    public class KeyInput : GameComponent, IKeyInput
    {
        #region Initialization
        public KeyInput(Game game) : base(game)
        {
            _keyboardInput = new Dictionary<Keys, Action<KeyboardState, GameTime>>();
            
            // registering itself as a service
            game.Services.AddService(typeof(IKeyInput), this);
        }
        #endregion

        #region Operations
        public override void Update(GameTime gameTime)
        {
            CheckKeyboardEvents(gameTime);
        }

        public virtual void Register(Keys key, Action<KeyboardState, GameTime> action)
        {
            _keyboardInput[key] = action;
        }

        public virtual Action<KeyboardState, GameTime> Unregister(Keys key)
        {
            Action<KeyboardState, GameTime> action = _keyboardInput[key];
            _keyboardInput.Remove(key);

            return action;
        }
        #endregion

        #region Event Detection
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
            return (last, current, gameTime) => last.IsKeyDown(key) && current.IsKeyUp(key);
        }
        #endregion

        #region Properties & Fields
        private KeyboardState LastKeyboardState
        {
            get { if (_lastKeyboardState == null) _lastKeyboardState = Keyboard.GetState(); return _lastKeyboardState; }
            set { _lastKeyboardState = value; }
        }
        private KeyboardState _lastKeyboardState;

        private IDictionary<Keys, Action<KeyboardState, GameTime>> _keyboardInput;
        #endregion
    }
    #endregion

    #region Mouse Input component
    public interface IMouseInput
    {
        void Register(MouseButtons mouseButton, Action<MouseState, GameTime> action);
        Action<MouseState, GameTime> Unregister(MouseButtons mouseButton);
    }

    public class MouseInput : Microsoft.Xna.Framework.GameComponent, IMouseInput
    {
        #region Initialization
        public MouseInput(Game game) : base(game)
        {
            _mouseInput = new Dictionary<MouseButtons, Action<MouseState, GameTime>>();

            // registering itself as a service
            game.Services.AddService(typeof(IMouseInput), this);
        }
        #endregion

        #region Operations
        public override void Update(GameTime gameTime)
        {
            CheckMouseEvents(gameTime);
        }

        public virtual void Register(MouseButtons mouseButton, Action<MouseState, GameTime> action)
        {
            _mouseInput[mouseButton] = action;
        }

        public virtual Action<MouseState, GameTime> Unregister(MouseButtons mouseButton)
        {
            if (mouseButton == null) // do nothing
                return null;

            Action<MouseState, GameTime> action = _mouseInput[mouseButton];
            _mouseInput.Remove(mouseButton);

            return action;
        }
        #endregion

        #region Event Detection
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
        
        private IDictionary<MouseButtons, Action<MouseState, GameTime>> _mouseInput;
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
}

