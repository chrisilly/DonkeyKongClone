using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DonkeyKongClone
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Texture2D tileTexture;

        List<string> levelRowList;
        Tile[,] levelTiles;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            tileTexture = Content.Load<Texture2D>("tile64");

            ReadLevel();
            LoadLevel();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
            StreamReader levelFile = new StreamReader("level.txt");
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
                    Color tileColor = Color.White;

                    if (levelRowList[i][j] == 'S')
                        tileColor = Color.Blue;
                    else if (levelRowList[i][j] == 'L')
                        tileColor = Color.Yellow;
                    else if (levelRowList[i][j] == 'F')
                        tileColor = Color.Orange;
                    else if (levelRowList[i][j] == 'P')
                        tileColor = Color.Pink;
                    else if (levelRowList[i][j] == 'M')
                        tileColor = Color.Red;
                    else if (levelRowList[i][j] == '-')
                        tileColor = Color.Black;

                    
                    levelTiles[j, i] = new Tile(tileTexture, new Vector2(Tile.tileSize.X * j, Tile.tileSize.Y * i), tileColor);
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
                }
            }
        }
    }
}