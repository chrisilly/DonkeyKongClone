using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DonkeyKongClone
{
    enum GameState { Menu, Play, Win, Lose }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Texture2D tileTexture;

        List<string> levelRowList;
        static Tile[,] levelTiles;

        Player player;
        List<Enemy> enemyList = new List<Enemy>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            ReadLevel();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileTexture = Content.Load<Texture2D>("tile64");

            LoadLevel();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            DrawLevel();
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        private void ReadLevel()
        {
            StreamReader levelFile = new StreamReader("..\\..\\..\\Content\\level.txt");
            levelRowList = new List<string>();

            while (!levelFile.EndOfStream)
            {
                levelRowList.Add(levelFile.ReadLine());
            }
            levelFile.Close();
        }

        private void LoadLevel()
        {
            levelTiles = new Tile[levelRowList[0].Length, levelRowList.Count];

            for (int i = 0; i < levelRowList.Count; i++)
            {
                for (int j = 0; j < levelRowList[0].Length; j++)
                {
                    Color tileColor = Color.Black;
                    Vector2 tilePosition = new Vector2(Tile.tileSize.X * j, Tile.tileSize.Y * i);

                    if (levelRowList[i][j] == 'S')
                    {
                        tileColor = Color.Blue;
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor, true, false);
                    }
                    else if (levelRowList[i][j] == 'L')
                    {
                        tileColor = Color.Yellow;
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor, false, true);
                    }
                    else if (levelRowList[i][j] == 'X')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor, true, false);
                    }
                    else if (levelRowList[i][j] == 'F')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                        Enemy enemy = new Enemy(tileTexture, tilePosition);
                        enemyList.Add(enemy);
                    }
                    else if (levelRowList[i][j] == 'P')
                    {
                        tileColor = Color.Pink;
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                    }
                    else if (levelRowList[i][j] == '-')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                    }
                    else if (levelRowList[i][j] == 'M')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                        player = new Player(tileTexture, tilePosition);
                    }
                }
            }
        }

        private void DrawLevel()
        {
            for (int i = 0; i < levelRowList.Count; i++)
            {
                for (int j = 0; j < levelRowList[0].Length; j++)
                {
                    levelTiles[j, i].Draw(spriteBatch);
                    foreach(Enemy enemy in enemyList)
                        enemy.Draw(spriteBatch);
                    player.Draw(spriteBatch);
                }
            }
        }

        public static bool IsTileValidPath(Vector2 position)
        {
            // Check if destination is not a solid tile
            if (!levelTiles[(int)position.X/Tile.tileSize.X, (int)position.Y/Tile.tileSize.Y].IsSolid())
            {
                // Check if the destination has a solid or ladder tile underneath it
                if (levelTiles[(int)position.X/Tile.tileSize.X, (int)(position.Y+Tile.tileSize.Y)/Tile.tileSize.Y].IsSolid() || levelTiles[(int)position.X / Tile.tileSize.X, (int)(position.Y + Tile.tileSize.Y) / Tile.tileSize.Y].IsLadder())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}