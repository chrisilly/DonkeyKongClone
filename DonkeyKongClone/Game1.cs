using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DonkeyKongClone
{
    enum GameState { Menu, Play, Win, Lose }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Random random = new Random();

        GameState gameState;

        List<string> levelRowList;
        static Tile[,] levelTiles;

        static int lives;

        Texture2D tileTexture;
        SpriteFont spriteFont;
        Vector2 hudPosition;

        string[] gameStateMessages = new string[] { "Press SPACE to play!", "You win! Press SPACE to play again!", "You lose! Press SPACE to play again!" };

        Player player;
        Actor pauline;
        List<Actor> enemyList = new List<Actor>();


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
            hudPosition = new Vector2(Tile.tileSize.X, Tile.tileSize.Y * levelRowList.Count);
            lives = 3;
            gameState = GameState.Menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteFont = Content.Load<SpriteFont>("spritefont");
            tileTexture = Content.Load<Texture2D>("tile64");

            LoadLevel();
            enemyList[random.Next(3)].SetSpeed(150f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameState.Menu:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        gameState = GameState.Play;
                    break;
                case GameState.Play:

                    if (lives <= 0)
                        gameState = GameState.Lose;

                    foreach (Actor enemy in enemyList)
                        enemy.Update(gameTime);

                    player.Update(gameTime);
                    CheckCollision();
                    break;
                case GameState.Win:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        RestartLevel();
                    break;
                case GameState.Lose:
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                        RestartLevel();
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            DrawTiles();

            switch (gameState)
            {
                case GameState.Menu:
                    Debug.WriteLine(gameStateMessages);
                    spriteBatch.DrawString(spriteFont, gameStateMessages[0], hudPosition, Color.White);
                    break;
                case GameState.Play:
                    foreach (Actor enemy in enemyList)
                        enemy.Draw(spriteBatch);
                    spriteBatch.DrawString(spriteFont, "Lives: " + lives, hudPosition, Color.White);
                    break;
                case GameState.Win:
                    spriteBatch.DrawString(spriteFont, gameStateMessages[1], hudPosition, Color.White);
                    break;
                case GameState.Lose:
                    spriteBatch.DrawString(spriteFont, gameStateMessages[2], hudPosition, Color.White);
                    break;
                default:
                    break;
            }

            pauline.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ReadLevel()
        {
            StreamReader levelFile = new StreamReader(@"..\..\..\Content\level.txt");
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
                    else if (levelRowList[i][j] == '-')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                    }
                    else if (levelRowList[i][j] == 'F')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                        Actor enemy = new Actor(tileTexture, tilePosition, Color.Orange);
                        enemyList.Add(enemy);
                    }
                    else if (levelRowList[i][j] == 'P')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                        pauline = new Actor(tileTexture, tilePosition, Color.Pink);
                    }
                    else if (levelRowList[i][j] == 'M')
                    {
                        levelTiles[j, i] = new Tile(tileTexture, tilePosition, tileColor);
                        player = new Player(tileTexture, tilePosition);
                    }
                }
            }
        }

        private void RestartLevel()
        {
            lives = 3;
            enemyList = new List<Actor>();
            LoadLevel();
            enemyList[random.Next(3)].SetSpeed(150f);
            gameState = GameState.Play;
        }

        private void DrawTiles()
        {
            for (int i = 0; i < levelRowList.Count; i++)
            {
                for (int j = 0; j < levelRowList[0].Length; j++)
                {
                    levelTiles[j, i].Draw(spriteBatch);
                }
            }
        }

        public static bool IsTileValidDestination(Vector2 destination)
        {
            // Check if destination is a non-solid tile
            if (!levelTiles[(int)destination.X / Tile.tileSize.X, (int)destination.Y / Tile.tileSize.Y].IsSolid())
            {
                // Check if the destination has a solid or ladder tile underneath it
                if (levelTiles[(int)destination.X / Tile.tileSize.X, (int)(destination.Y + Tile.tileSize.Y) / Tile.tileSize.Y].IsSolid() || levelTiles[(int)destination.X / Tile.tileSize.X, (int)(destination.Y + Tile.tileSize.Y) / Tile.tileSize.Y].IsLadder())
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

        public void CheckCollision()
        {
            foreach (Actor enemy in enemyList)
            {
                if (player.GetHitbox().Intersects(enemy.GetHitbox()))
                {
                    lives--;
                    enemyList.Remove(enemy);
                    break;
                }
            }

            if (player.GetHitbox().Intersects(pauline.GetHitbox()))
            {
                gameState = GameState.Win;
            }
        }
    }
}