using System;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife
{
    public static class Utilities
    {
        public static void DrawInScope(this SpriteBatch batch, Action<SpriteBatch> action)
        {
            batch.Begin();

            action(batch);

            batch.End();
        }
    }
}
