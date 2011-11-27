using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameOfLife;
using GameOfLife.Input;

namespace GameOfLife
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        InputManager input;
        
        GameState gameState;
        Graphics graphics;

        public MainGame()
        {
            // game components
            gameState = new GameState(this, new World(25, 25));
            Components.Add(gameState);

            graphics = new Graphics(this);
            Components.Add(graphics);

            // rest
            input = new InputManager();
            input.CellToggle += (sender, args) => gameState.World.Toggle(XToRow(args.Current.X), YToColumn(args.Current.Y));
            input.ExecutionToggle += 
                (sender, args) => {
                    gameState.Running = !gameState.Running;
                    gameState._timeOfLastTick = args.GameTime.TotalGameTime;
                };
            input.QuitGame += (sender, args) => Exit();

            

            IsMouseVisible = true;
            

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: Load any non ContentManager content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update(gameTime);

            base.Update(gameTime);
        }

        private int XToRow(int x)
        {
            return (int) (x * gameState.World.RowCount / graphics.Width);
        }

        private int YToColumn(int y) 
        { 
            return (int) (y * gameState.World.ColumnCount / graphics.Height);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
