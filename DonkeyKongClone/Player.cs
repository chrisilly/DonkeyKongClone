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
    internal class Player
    {
        Texture2D texture;

        Vector2 position;
        Vector2 direction;
        Vector2 destination;
        float speed = 225f;
        bool moving = false;

        Rectangle hitbox;

        public Player(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            Controller(gameTime);
            UpdateHitbox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.Red);
            //spriteBatch.Draw(texture, hitbox, Color.FromNonPremultiplied(0, 255, 0, 255)); // Draw Player Hitbox
        }

        public void Controller(GameTime gameTime)
        {
            if (!moving)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    Move(new Vector2(-1, 0));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    Move(new Vector2(1, 0));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    Move(new Vector2(0, -1));
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    Move(new Vector2(0, 1));
                }
            }
            else
            {
                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (Vector2.Distance(position, destination) < 1)
                {
                    position = destination;
                    moving = false;
                }
            }
        }

        public void Move(Vector2 direction)
        {
            this.direction = direction;
            Vector2 newDestination = position + direction * Tile.tileSize.X;

            if (Game1.IsTileValidDestination(newDestination))
            {
                destination = newDestination;
                moving = true;
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
    }
}
