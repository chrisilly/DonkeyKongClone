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
    internal class Actor
    {
        Random random = new Random();

        Texture2D texture;
        Color color;

        Vector2 position;
        Vector2 direction;
        Vector2 destination;
        float speed; // There needs to be enemies with different speeds
        bool fast;

        Rectangle hitbox;

        public Actor(Texture2D texture, Vector2 position, Color color)
        {
            this.texture = texture;
            this.position = position;
            this.color = color;
            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            speed = 50f;
            if (random.Next(0,1) == 0)
                direction = new Vector2(-1, 0);
            else
                direction = new Vector2(1, 0);
            destination = position;
        }

        public void Update(GameTime gameTime)
        {
            UpdateHitbox();

            Move(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }

        public void Move(GameTime gameTime)
        {
            if (position == destination)
            {
                Vector2 newDestination = position + direction * Tile.tileSize.X;

                if (Game1.IsTileValidDestination(newDestination))
                    destination = newDestination;
                else
                    direction *= -1;
            }
            else
            {
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Vector2.Distance(position, destination) < 2)
                {
                    position = destination;
                }
            }
        }

        public void UpdateHitbox()
        {
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
        }

        public Rectangle GetHitbox()
        {
            return hitbox;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
    }
}
