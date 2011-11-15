using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameOfLife.Events;

namespace GameOfLife
{
    public static class Utilities
    {
        public static void DrawInScope(this SpriteBatch batch, Action<SpriteBatch> action)
        {
            batch.Begin();

            action(batch);

            batch.End();
        }
    }

    public class InputManager
    {
        #region Operations
        public virtual void Update(GameTime gameTime)
        {
            CheckMouseEvents(gameTime);
            CheckKeyboardEvents(gameTime);
        }

        protected void CheckKeyboardEvents(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (Func<KeyboardState, KeyboardState, bool> e in KeyboardObservers.Keys)
            {
                if (e(LastKeyboardState, keyboardState))
                    KeyboardObservers[e](keyboardState, gameTime);
            }

            LastKeyboardState = keyboardState;
        }
        #endregion

        #region Events
        public event EventHandler<MouseEventArgs> LeftButtonClicked;

        protected void CheckMouseEvents(GameTime gameTime) 
        {
            MouseState current = Mouse.GetState();

            if (current.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed)
                LeftButtonClicked(this, new MouseEventArgs(LastMouseState, current, gameTime));

            LastMouseState = current;
        }
        #endregion

        #region Observers
        public IDictionary<Func<KeyboardState, KeyboardState, bool>, Action<KeyboardState, GameTime>> KeyboardObservers
        {
            get
            {
                keyboardObservers = keyboardObservers ?? new Dictionary<Func<KeyboardState, KeyboardState, bool>, Action<KeyboardState, GameTime>>();
                return keyboardObservers;
            }
            set { keyboardObservers = value; }
        }

        private IDictionary<Func<MouseState, MouseState, bool>, Action<MouseState, GameTime>> mouseObservers;
        private IDictionary<Func<KeyboardState, KeyboardState, bool>, Action<KeyboardState, GameTime>> keyboardObservers;
        #endregion

        #region Properties & Fields
        private MouseState LastMouseState
        {
            get { if (lastMouseState == null) lastMouseState = Mouse.GetState(); return lastMouseState; }
            set { lastMouseState = value; }
        }

        private KeyboardState LastKeyboardState
        {
            get { if (lastKeyboardState == null) lastKeyboardState = Keyboard.GetState(); return lastKeyboardState; }
            set { lastKeyboardState = value; }
        }

        private MouseState lastMouseState;
        private KeyboardState lastKeyboardState;
        #endregion
    }

    namespace Events
    {
        public class MouseEventArgs : EventArgs
        {
            public MouseEventArgs(MouseState last, MouseState current, GameTime gameTime)
            {
                Last = last;
                Current = current;
                GameTime = gameTime;
            }

            #region Properties & Fields
            private MouseState last;
            private MouseState current;
            private GameTime gameTime;

            public MouseState Last
            {
                get { return last; }
                private set { last = value; }
            }

            public MouseState Current
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
    }
}
