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


namespace GameOfLife
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Graphics : Microsoft.Xna.Framework.DrawableGameComponent, IGraphicsDeviceService
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D dummyTexture;
        Rectangle dummyRectangle;

        readonly Color staticColor = Color.White;
        readonly Color runningColor = Color.Wheat;
        readonly int cellWidth = 16;
        readonly int cellHeight = 16;

        public Graphics(Game game) : base(game)
        {
            graphics = new GraphicsDeviceManager(game);
            dummyRectangle = new Rectangle(0, 0, cellWidth, cellHeight);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            // Query for the graphics device service through which the graphics device
            // can be accessed
            IGameState gameState = (IGameState) Game.Services.GetService(typeof(IGameState));

            graphics.PreferredBackBufferWidth = gameState.World.ColumnCount * cellWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = gameState.World.RowCount * cellHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            IGameState gameState = (IGameState)Game.Services.GetService(typeof(IGameState));

            GraphicsDevice.Clear(gameState.Running ? runningColor : staticColor);

            // drawing the world
            spriteBatch.DrawInScope((self) =>
            {
                for (int i = 0; i < gameState.World.RowCount; i++)
                {
                    for (int j = 0; j < gameState.World.ColumnCount; j++)
                    {
                        if (gameState.World.IsAlive(i, j))
                            self.Draw(dummyTexture, new Vector2(RowToX(i), ColumnToY(j)), dummyRectangle, Color.Black);
                    }
                }
            });

            base.Draw(gameTime);
        }

        private int ColumnToY(int column)
        {
            IGameState gameState = (IGameState) Game.Services.GetService(typeof(IGameState));
            return (int)(column * graphics.PreferredBackBufferHeight / gameState.World.ColumnCount);
        }

        private int RowToX(int row)
        {
            IGameState gameState = (IGameState) Game.Services.GetService(typeof(IGameState));
            return (int)(row * graphics.PreferredBackBufferWidth / gameState.World.RowCount);
        }

        #region Events That Should've Have Been Inherited
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;
        #endregion

        #region Properties & Fields
        public int Width
        {
            get { return graphics.PreferredBackBufferWidth; }
        }

        public int Height
        {
            get { return graphics.PreferredBackBufferHeight; }
        }
        #endregion
    }
}
