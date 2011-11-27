﻿using System;
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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D dummyTexture;
        Rectangle dummyRectangle;

        InputManager input;
        
        readonly Color staticColor = Color.White;
        readonly Color runningColor = Color.Wheat;
        readonly int rows = 25;
        readonly int columns = 25;
        readonly int cellWidth = 16;
        readonly int cellHeight = 16;

        GameState gameState;

        public MainGame()
        {
            // game components
            gameState = new GameState(this, new World(rows, columns));
            Components.Add(gameState);

            // rest
            input = new InputManager();
            input.CellToggle += (sender, args) => gameState.World.Toggle(XToRow(args.Current.X), YToColumn(args.Current.Y));
            input.ExecutionToggle += 
                (sender, args) => {
                    gameState.Running = !gameState.Running;
                    gameState._timeOfLastTick = args.GameTime.TotalGameTime;
                };
            input.QuitGame += (sender, args) => Exit();

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = columns * cellWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = rows * cellHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            IsMouseVisible = true;

            dummyRectangle = new Rectangle(0, 0, cellWidth, cellHeight);
            

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
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
            return (int) (x * rows / graphics.PreferredBackBufferWidth);
        }

        private int YToColumn(int y) 
        { 
            return (int) (y * columns / graphics.PreferredBackBufferHeight);
        }

        private int ColumnToY(int column) 
        {
            return (int) (column * graphics.PreferredBackBufferHeight / columns);
        }

        private int RowToX(int row)
        {
            return (int)(row * graphics.PreferredBackBufferWidth / rows);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(gameState.Running ? runningColor : staticColor);

            // drawing the world
            spriteBatch.DrawInScope((self) => {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (gameState.World[i, j] == World.CellState.Alive)
                            self.Draw(dummyTexture, new Vector2(RowToX(i), ColumnToY(j)), dummyRectangle, Color.Black);
                    }
                }   
            });

            base.Draw(gameTime);
        }
    }
}
