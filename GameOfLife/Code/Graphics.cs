using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameOfLife.GameState;
using GameOfLife.Settings;

namespace GameOfLife.Graphics
{
    public interface IView : IGraphicsDeviceService
    {
        int XToRow(int x);
        int YToColumn(int y);
        int RowToX(int row);
        int ColumnToY(int column);

        bool DrawGrid { get; set; }
        int CellWidth { get; }
        int CellHeight { get; }
    }
    
    public class View : Microsoft.Xna.Framework.DrawableGameComponent, IView
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
            backgroundColor = staticColor;

            // registering itself as a service
            game.Services.AddService(typeof(IView), this);
        }

        public override void Initialize()
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            ISettings settings = (ISettings) Game.Services.GetService(typeof(ISettings));

            CellWidth = settings.CellWidth;
            CellHeight = settings.CellHeight;

            dummyRectangle = new Rectangle(0, 0, CellWidth, CellHeight);
            
            graphics.PreferredBackBufferWidth = gameState.World.ColumnCount * CellWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = gameState.World.RowCount * CellHeight;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            gameState.RunningToggled += (sender, args) => backgroundColor = args.Current ? runningColor : staticColor;

            backgroundColor = settings.RunAtStart ? runningColor : staticColor;
            DrawGrid = settings.DrawGridAtStart;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // dummy texture to paint the cells
            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));

            GraphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin();

                for (int i = 0; i < gameState.World.RowCount; i++)
                {
                    for (int j = 0; j < gameState.World.ColumnCount; j++)
                    {
                        if (gameState.World.IsAlive(i, j))
                            spriteBatch.Draw(dummyTexture, new Vector2(RowToX(i), ColumnToY(j)), dummyRectangle, Color.Black);
                    }
                }

                if (DrawGrid)
                {
                    ISettings settings = (ISettings) Game.Services.GetService(typeof(ISettings));
                    for (int i = 0; i < gameState.World.RowCount; i++)
                    {
                        DrawLine(spriteBatch, dummyTexture, 1, settings.GridColor, new Vector2(0, RowToX(i)), new Vector2(Width, RowToX(i)));
                    }

                    for (int j = 0; j < gameState.World.ColumnCount; j++)
                    {
                        DrawLine(spriteBatch, dummyTexture, 1, settings.GridColor, new Vector2(ColumnToY(j), 0), new Vector2(ColumnToY(j), Height));
                    }
                }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Helper Methods
        private void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float) Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
        #endregion

        #region IView Methods
        public virtual int ColumnToY(int column)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(column * graphics.PreferredBackBufferHeight / gameState.World.ColumnCount);
        }

        public virtual int RowToX(int row)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(row * graphics.PreferredBackBufferWidth / gameState.World.RowCount);
        }

        public virtual int XToRow(int x)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(x * gameState.World.RowCount / Width);
        }

        public virtual int YToColumn(int y)
        {
            IState gameState = (IState) Game.Services.GetService(typeof(IState));
            return (int)(y * gameState.World.ColumnCount / Height);
        }

        public bool DrawGrid
        {
            get { return _drawGrid; }
            set { _drawGrid = value; }
        }
        private bool _drawGrid;

        public int CellWidth 
        {
            get { return _cellWidth; }
            private set { _cellWidth = value; }
        }
        private int _cellWidth;

        public int CellHeight
        {
            get { return _cellHeight; }
            private set { _cellHeight = value; }
        }
        private int _cellHeight;
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
