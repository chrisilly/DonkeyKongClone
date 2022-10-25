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

        public Player(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Update(GameTime gameTime)
        {
            Controller(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.Red);
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
            //Vector2 newDestination = position + direction * Tile.tileSize.X;

            //destination = newDestination;

            destination = position + direction * (float)Tile.tileSize.X;
            moving = true;
        }
    }
}
