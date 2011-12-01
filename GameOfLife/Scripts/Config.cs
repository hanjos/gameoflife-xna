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
    public class Config
    {
        public int Rows;
        public int Columns;
        public int TickInMilliseconds;
        public bool StartRunning;
        public Input[] Inputs;
    }

    public class Input
    {
        public string Key;
        public string Command;
    }
}
