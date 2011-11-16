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
        #endregion

        #region Events
        public event EventHandler<InputEventArgs<MouseState>> LeftButtonClicked;
        public event EventHandler<InputEventArgs<KeyboardState>> SpacePressed;
        public event EventHandler<InputEventArgs<KeyboardState>> EscapePressed;

        protected void CheckMouseEvents(GameTime gameTime) 
        {
            MouseState current = Mouse.GetState();

            if (current.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed)
                RaiseLeftButtonClicked(new InputEventArgs<MouseState>(LastMouseState, current, gameTime));

            LastMouseState = current;
        }

        protected void CheckKeyboardEvents(GameTime gameTime)
        {
            KeyboardState current = Keyboard.GetState();

            if (current.IsKeyUp(Keys.Space) && LastKeyboardState.IsKeyDown(Keys.Space))
                RaiseSpacePressed(new InputEventArgs<KeyboardState>(LastKeyboardState, current, gameTime));

            if (current.IsKeyUp(Keys.Escape) && LastKeyboardState.IsKeyDown(Keys.Escape))
                RaiseEscapePressed(new InputEventArgs<KeyboardState>(LastKeyboardState, current, gameTime));

            LastKeyboardState = current;
        }

        // for derived classes to use
        protected virtual void RaiseLeftButtonClicked(InputEventArgs<MouseState> args)
        {
            if (LeftButtonClicked != null)
                LeftButtonClicked(this, args);
        }

        // for derived classes to use
        protected virtual void RaiseSpacePressed(InputEventArgs<KeyboardState> args)
        {
            if (SpacePressed != null)
                SpacePressed(this, args);
        }

        // for derived classes to use
        protected virtual void RaiseEscapePressed(InputEventArgs<KeyboardState> args)
        {
            if (EscapePressed != null)
                EscapePressed(this, args);
        }
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
        public class InputEventArgs<T> : EventArgs
        {
            public InputEventArgs(T last, T current, GameTime gameTime)
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
    }
}
