using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameOfLife.Input;
using GameOfLife.GameState;

namespace GameOfLife.Graphics
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class View : Microsoft.Xna.Framework.DrawableGameComponent, IGraphicsDeviceService
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D dummyTexture;
        Rectangle dummyRectangle;
        Color backgroundColor;

        readonly Color staticColor = Color.White;
        readonly Color runningColor = Color.Wheat;
        readonly int cellWidth = 16;
        readonly int cellHeight = 16;

        public View(Game game) : base(game)
        {
            graphics = new GraphicsDeviceManager(game);
            dummyRectangle = new Rectangle(0, 0, cellWidth, cellHeight);
            backgroundColor = staticColor;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // Query for the graphics device service through which the graphics device
            // can be accessed
            IState gameState = (IState) Game.Services.GetService(typeof(IState));

            graphics.PreferredBackBufferWidth = gameState.World.ColumnCount * cellWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = gameState.World.RowCount * cellHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            gameState.RunningToggled += (sender, args) => backgroundColor = args.Current ? runningColor : staticColor;

            IInput input = (IInput) Game.Services.GetService(typeof(IInput));
            
            input.CellToggle += (sender, args) => gameState.World.Toggle(XToRow(args.Current.X), YToColumn(args.Current.Y));

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

        public override void Draw(GameTime gameTime)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));

            GraphicsDevice.Clear(backgroundColor);

            // drawing the world
            spriteBatch.Begin();

                for (int i = 0; i < gameState.World.RowCount; i++)
                {
                    for (int j = 0; j < gameState.World.ColumnCount; j++)
                    {
                        if (gameState.World.IsAlive(i, j))
                            spriteBatch.Draw(dummyTexture, new Vector2(RowToX(i), ColumnToY(j)), dummyRectangle, Color.Black);
                    }
                }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Helper Methods
        private int ColumnToY(int column)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(column * graphics.PreferredBackBufferHeight / gameState.World.ColumnCount);
        }

        private int RowToX(int row)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(row * graphics.PreferredBackBufferWidth / gameState.World.RowCount);
        }

        private int XToRow(int x)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(x * gameState.World.RowCount / Width);
        }

        private int YToColumn(int y)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(y * gameState.World.ColumnCount / Height);
        }
        #endregion

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
