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
        bool solid;

        public static Point tileSize = new Point(64, 64);

        // For solid tiles which actors collide with
        public Tile(Texture2D tileTexture, Vector2 tilePosition, Color tileColor, bool solid)
        {
            this.tileTexture = tileTexture;
            this.tilePosition = tilePosition;
            this.tileColor = tileColor;
            this.solid = solid;
        }
        
        // For non-solids that do not collide
        public Tile(Texture2D tileTexture, Vector2 tilePosition, Color tileColor)
        {
            this.tileTexture = tileTexture;
            this.tilePosition = tilePosition;
            this.tileColor = tileColor;
            this.solid = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tileTexture, tilePosition, tileColor);
        }
    }
}
