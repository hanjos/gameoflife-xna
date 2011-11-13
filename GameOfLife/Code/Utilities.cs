using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        protected void CheckMouseEvents(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            foreach (Func<MouseState, MouseState, bool> e in MouseObservers.Keys)
            {
                if (e(LastMouseState, mouseState))
                    MouseObservers[e](mouseState, gameTime);
            }

            LastMouseState = mouseState;
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

        #region Observers
        public IDictionary<Func<MouseState, MouseState, bool>, Action<MouseState, GameTime>> MouseObservers
        {
            get { mouseObservers = mouseObservers ?? new Dictionary<Func<MouseState, MouseState, bool>, Action<MouseState, GameTime>>(); return mouseObservers; }
            set { mouseObservers = value; }
        }

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
}
