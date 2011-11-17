using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameOfLife.Events
{
    public class CellToggled
    {
        public CellToggled(int row, int column, GameTime gameTime)
        {
            Row = row; 
            Column = column;
            GameTime = gameTime;
        }

        #region Properties & Events
        public int Row
        {
            get { return row_; }
            private set { row_ = value; }
        }

        public int Column
        {
            get { return column_; }
            private set { column_ = value; }
        }

        public GameTime GameTime
        {
            get { return gameTime_; }
            private set { gameTime_ = value; }
        }

        private int row_;
        private int column_;
        private GameTime gameTime_;
        #endregion
    }

    public class RunningToggled
    {
        public RunningToggled(GameTime gameTime)
        {
            GameTime = gameTime;
        }

        #region Properties & Events
        public GameTime GameTime
        {
            get { return gameTime_; }
            private set { gameTime_ = value; }
        }

        private GameTime gameTime_;
        #endregion
    }

    public class GameQuit
    {
        public GameQuit(GameTime gameTime)
        {
            GameTime = gameTime;
        }

        #region Properties & Events
        public GameTime GameTime
        {
            get { return gameTime_; }
            private set { gameTime_ = value; }
        }

        private GameTime gameTime_;
        #endregion
    }
}
