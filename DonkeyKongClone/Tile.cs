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
        Texture2D texture;
        Vector2 position;
        Color color;

        bool solid;
        bool ladder;

        public static Point tileSize = new Point(64, 64);

        // For solid and ladder tiles which actors collide and interact with
        public Tile(Texture2D texture, Vector2 position, Color color, bool solid, bool ladder)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            this.solid = solid;
            this.ladder = ladder;
        }
        
        // For tiles that do not collide or interact with the player
        public Tile(Texture2D texture, Vector2 position, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            this.solid = false;
            this.ladder = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }

        public bool IsSolid()
        {
            return solid;
        }

        public bool IsLadder()
        {
            return ladder;
        }
    }
}
