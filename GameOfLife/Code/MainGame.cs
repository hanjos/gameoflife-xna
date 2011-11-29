using Microsoft.Xna.Framework;
using GameOfLife.Graphics;
using GameOfLife.Input;
using GameOfLife.Model;
using GameOfLife.GameState;

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
           base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
