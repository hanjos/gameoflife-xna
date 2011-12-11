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

namespace Scripts
{
    public struct Config
    {
        public int Rows;
        public int Columns;
        public int TickInMilliseconds;
        public bool RunAtStart;
        public bool DrawGridLines;
        public Commands Commands;
    }

    public struct Commands
    {
        public string ToggleCell;
        public string ToggleRunning;
        public string ToggleGridLines;
        public string SpeedUp;
        public string SlowDown;
        public string Quit;
    }
}
