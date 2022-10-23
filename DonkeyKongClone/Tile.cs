using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKongClone
{
    internal class Tile
    {
        Texture2D tileTexture;
        Vector2 tilePosition;
        Color tileColor;

        public static Point tileSize = new Point(64, 64);

        public Tile(Texture2D tileTexture, Vector2 tilePosition, Color tileColor)
        {
            this.tileTexture = tileTexture;
            this.tilePosition = tilePosition;
            this.tileColor = tileColor;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileTexture, tilePosition, tileColor);
        }
    }
}
