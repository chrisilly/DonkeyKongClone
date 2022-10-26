using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKongClone
{
    internal class Enemy
    {
        Texture2D texture;
        Vector2 position;
        Vector2 direction;
        Vector2 destination;
        float speed; // There needs to be enemies with different speeds

        public Enemy(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.Orange);
        }
    }
}
