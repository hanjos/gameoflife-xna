using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
    namespace Input
    {
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
            public event EventHandler<InputEventArgs<MouseState>> CellToggle;
            public event EventHandler<InputEventArgs<KeyboardState>> ExecutionToggle;
            public event EventHandler<InputEventArgs<KeyboardState>> QuitGame;

            protected void CheckMouseEvents(GameTime gameTime)
            {
                MouseState current = Mouse.GetState();

                if (DetectMouseClicked(MouseKeys.LeftButton)(LastMouseState, current))
                    RaiseLeftButtonClicked(new InputEventArgs<MouseState>(LastMouseState, current, gameTime));

                LastMouseState = current;
            }

            protected void CheckKeyboardEvents(GameTime gameTime)
            {
                KeyboardState current = Keyboard.GetState();

                if (DetectKeyPressed(Keys.Space)(LastKeyboardState, current))
                    RaiseSpacePressed(new InputEventArgs<KeyboardState>(LastKeyboardState, current, gameTime));

                if (DetectKeyPressed(Keys.Escape)(LastKeyboardState, current))
                    RaiseEscapePressed(new InputEventArgs<KeyboardState>(LastKeyboardState, current, gameTime));

                LastKeyboardState = current;
            }

            protected Func<KeyboardState, KeyboardState, bool> DetectKeyPressed(Keys key)
            {
                return (KeyboardState last, KeyboardState current) => last.IsKeyDown(key) && current.IsKeyUp(key);
            }

            protected Func<MouseState, MouseState, bool> DetectMouseClicked(MouseKeys key)
            {
                return key.Detect;
            }

            // for derived classes to use
            protected virtual void RaiseLeftButtonClicked(InputEventArgs<MouseState> args)
            {
                if (CellToggle != null)
                    CellToggle(this, args);
            }

            // for derived classes to use
            protected virtual void RaiseSpacePressed(InputEventArgs<KeyboardState> args)
            {
                if (ExecutionToggle != null)
                    ExecutionToggle(this, args);
            }

            // for derived classes to use
            protected virtual void RaiseEscapePressed(InputEventArgs<KeyboardState> args)
            {
                if (QuitGame != null)
                    QuitGame(this, args);
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

        public abstract class MouseKeys
        {
            public static readonly MouseKeys LeftButton = new LeftButtonMouseKeys();
            public static readonly MouseKeys RightButton = new RightButtonMouseKeys();

            public abstract bool Detect(MouseState last, MouseState current);

            private class LeftButtonMouseKeys : MouseKeys
            {
                public override bool Detect(MouseState last, MouseState current)
                {
                    return last.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released;
                }
            }

            private class RightButtonMouseKeys : MouseKeys
            {
                public override bool Detect(MouseState last, MouseState current)
                {
                    return last.RightButton == ButtonState.Pressed && current.RightButton == ButtonState.Released;
                }
            }
        }

        

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
