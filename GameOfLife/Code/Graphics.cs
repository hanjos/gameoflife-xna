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
        Color DeadColor { get; }
        Color RunningColor { get; }
        Color LiveColor { get; }
    }
    
    public class View : Microsoft.Xna.Framework.DrawableGameComponent, IView
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D dummyTexture;
        Rectangle dummyRectangle;
        
        public View(Game game) : base(game)
        {
            graphics = new GraphicsDeviceManager(game);
            
            // registering itself as a service
            game.Services.AddService(typeof(IView), this);
        }

        public override void Initialize()
        {
            IState state = (IState) Game.Services.GetService(typeof(IState));
            ISettings settings = (ISettings) Game.Services.GetService(typeof(ISettings));

            // load settings
            CellWidth = settings.CellWidth;
            CellHeight = settings.CellHeight;
            DeadColor = settings.DeadColor;
            RunningColor = settings.RunningColor;
            LiveColor = settings.LiveColor;
            DrawGrid = settings.DrawGridAtStart;

            // prep for rectangle drawing
            dummyRectangle = new Rectangle(0, 0, CellWidth, CellHeight);
            
            // window size
            graphics.PreferredBackBufferWidth = state.World.ColumnCount * CellWidth;
            graphics.PreferredBackBufferHeight = state.World.RowCount * CellHeight;
            graphics.ApplyChanges();

            // registering a callback in the model
            state.RunningToggled += (sender, args) => BackgroundColor = args.Current ? RunningColor : DeadColor;

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
            IState state = (IState) Game.Services.GetService(typeof(IState));

            GraphicsDevice.Clear(BackgroundColor);

            spriteBatch.Begin();

                for (int i = 0; i < state.World.RowCount; i++)
                {
                    for (int j = 0; j < state.World.ColumnCount; j++)
                    {
                        if (state.World.IsAlive(i, j))
                            spriteBatch.Draw(dummyTexture, new Vector2(RowToX(i), ColumnToY(j)), dummyRectangle, LiveColor);
                    }
                }

                if (DrawGrid)
                {
                    ISettings settings = (ISettings) Game.Services.GetService(typeof(ISettings));
                    for (int i = 0; i < state.World.RowCount; i++)
                    {
                        DrawLine(spriteBatch, dummyTexture, 1, settings.GridColor,
                            new Vector2(0, i * CellHeight), new Vector2(Width, i * CellHeight));
                    }

                    for (int j = 0; j < state.World.ColumnCount; j++)
                    {
                        DrawLine(spriteBatch, dummyTexture, 1, settings.GridColor,
                            new Vector2(j * CellWidth, 0), new Vector2(j * CellWidth, Height));
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

        public Color DeadColor
        {
            get { return _deadColor; }
            private set { _deadColor = value; }
        }
        private Color _deadColor;

        public Color RunningColor
        {
            get { return _runningColor; }
            private set { _runningColor = value; }
        }
        private Color _runningColor;

        public Color LiveColor
        {
            get { return _liveColor; }
            private set { _liveColor = value; }
        }
        private Color _liveColor;
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

        public Color BackgroundColor
        {
            get 
            {
                if (_firstInvocationOfBackgroundColor)
                {
                    _firstInvocationOfBackgroundColor = false;

                    ISettings settings = (ISettings) Game.Services.GetService(typeof(ISettings));
                    BackgroundColor = settings.RunAtStart ? settings.RunningColor : settings.DeadColor;
                }

                return _backgroundColor;
            }
            set { _backgroundColor = value; }
        }
        private Color _backgroundColor;
        private bool _firstInvocationOfBackgroundColor = true; // HACK
        #endregion

        #region Events That Should've Have Been Inherited
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;
        #endregion
    }
}
