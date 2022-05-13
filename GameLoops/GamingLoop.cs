using Gonk_Konk_Complete_Edition.Sprites.GameSprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Gonk_Konk_Complete_Edition.Sprites.MenuSprites;

namespace Gonk_Konk_Complete_Edition.GameLoops
{
    public class GamingLoop
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont spriteFont;

        private Player player;
        public bool Loser = false;
        private bool playingLoseMusic = false;

        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();
        List<Konk> konks = new List<Konk>();

        // The game camera
        CirclingCamera camera;
        Game game;

        float unmodifiedScore = 0;
        float score = 0;
        float accuracy = 0;
        int TotalBullets = 0;
        int TotalHits = 0;

        KeyboardState keyboardCurrent;
        KeyboardState keyboardPrior = Keyboard.GetState();

        System.Random random;
        float spawnTimer = 0.0f;

        private SoundEffect enemyDeath1;
        private SoundEffect enemyDeath2;
        private SoundEffect enemyDeath3;
        private SoundEffect pew;
        private Song backgroundMusic;

        private MainLoop parent;

        private GenericSprite[] sprites;


        public GamingLoop(Game gamu, MainLoop p)
        {
            game = gamu;
            parent = p;
        }

        public void Initialize(ContentManager content)
        {
            random = new System.Random(DateTime.Now.Millisecond);


            //declare the sprite classes
            player = new Player();
            Konk3D konk;
            konk = new Konk3D(game, Matrix.CreateTranslation(0, -3.75f, 0));
            konks.Add(new Konk(content, 0, konk));
            konk = new Konk3D(game, Matrix.CreateTranslation(0, -1.625f, 0));
            konks.Add(new Konk(content, 1, konk));
            konk = new Konk3D(game, Matrix.CreateTranslation(0, 0f, 0));
            konks.Add(new Konk(content, 2, konk));
            konk = new Konk3D(game, Matrix.CreateTranslation(0, 1.625f, 0));
            konks.Add(new Konk(content, 3, konk));
            konk = new Konk3D(game, Matrix.CreateTranslation(0, 3.25f, 0));
            konks.Add(new Konk(content, 4, konk));

            sprites = new GenericSprite[200];

            int ran;
            for (int i = 0; i < 200; i++)
            {
                ran = random.Next(1, 3);
                switch (ran)
                {
                    case 1:
                        sprites[i] = new GenericSprite("star1", new Vector2(random.Next(0, Constants.GAME_WIDTH), random.Next(0, Constants.GAME_HEIGHT)), 3, 3);
                        break;
                    case 2:
                        sprites[i] = new GenericSprite("star2", new Vector2(random.Next(0, Constants.GAME_WIDTH), random.Next(0, Constants.GAME_HEIGHT)), 3, 3);
                        break;
                    case 3:
                        sprites[i] = new GenericSprite("star3", new Vector2(random.Next(0, Constants.GAME_WIDTH), random.Next(0, Constants.GAME_HEIGHT)), 3, 3);
                        break;
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            camera = new CirclingCamera(game, new Vector3(0, 3, 10), 0.5f);


            // TODO: use this.Content to load your game content here
            player.LoadContent(content);
            enemyDeath1 = content.Load<SoundEffect>("GameContent/Legoyodadeathsound");
            enemyDeath2 = content.Load<SoundEffect>("GameContent/legolukeskywalkerdeathsound");
            enemyDeath3 = content.Load<SoundEffect>("GameContent/legodarthvaderdeathsound");
            pew = content.Load<SoundEffect>("GameContent/gunshot");

            backgroundMusic = content.Load<Song>("GameContent/TTFAF");
            MediaPlayer.Volume = 0.10f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);

            foreach (GenericSprite s in sprites)
            {
                s.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime, ContentManager content)
        {
            if (!Loser)
            {
                camera.Update(gameTime);

                player.Update(gameTime);

                //handle input for gun
                keyboardCurrent = Keyboard.GetState();
                if (keyboardCurrent.IsKeyDown(Keys.Space) && !(keyboardPrior.IsKeyDown(Keys.Space)))
                {
                    if (player.facingRight)
                    {
                        bullets.Add(new Bullet(content, player.Position + new Vector2(90, -15), player.facingRight));
                        TotalBullets++;
                    }
                    else
                    {
                        bullets.Add(new Bullet(content, player.Position + new Vector2(-155, -15), player.facingRight));
                        TotalBullets++;
                    }
                    pew.Play(0.7f, 0, 0);
                }
                keyboardPrior = keyboardCurrent;

                if(score > parent.HighScore)
                {
                    parent.HighScore = (int)score;
                }
            }
            else
            {
                if (!playingLoseMusic)
                {
                    playingLoseMusic = true;
                    backgroundMusic = content.Load<Song>("GameContent/sad");
                    MediaPlayer.Play(backgroundMusic);
                }
            }
            if (spawnTimer <= 0)
            {
                enemies.Add(new Enemy(content));
                spawnTimer = (float)random.Next(3, 7) / 10;
            }
            else
            {
                spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //handle bullet positions
            foreach (Bullet b in bullets)
            {
                b.Update(gameTime);
            }
            foreach (Enemy e in enemies)
            {
                e.Update(gameTime);
            }

            //check for collision between enemies and bullets
            List<Bullet> bToRemove = new List<Bullet>();
            List<Enemy> eToRemove = new List<Enemy>();
            foreach (Bullet b in bullets)
            {
                foreach (Enemy e in enemies)
                {
                    if (b.CollidesWith(e.CollisionBox))
                    {
                        bToRemove.Add(b);
                        eToRemove.Add(e);

                        TotalHits++;

                        unmodifiedScore += 100;
                        
                    }
                }
            }
            if(TotalBullets > 0)
            {
                accuracy = (float)TotalHits / (float)TotalBullets;
                score = unmodifiedScore * (float)Math.Round(accuracy, 2);
            }
            

            foreach (Bullet b in bToRemove)
            {
                bullets.Remove(b);
            }
            foreach (Enemy e in eToRemove)
            {
                enemies.Remove(e);
                int ran = random.Next(1, 4);
                switch (ran)
                {
                    case 1:
                        enemyDeath1.Play(0.1f, 0, 0);
                        break;
                    case 2:
                        enemyDeath2.Play(0.1f, 0, 0);
                        break;
                    case 3:
                        enemyDeath3.Play(0.05f, 0, 0);
                        break;
                }
            }
            bToRemove.Clear();
            eToRemove.Clear();

            //check for collisions between enemies and konks
            List<Konk> kToRemove = new List<Konk>();
            foreach (Enemy e in enemies)
            {
                foreach (Konk k in konks)
                {
                    if (e.CollidesWith(k.CollisionBox))
                    {
                        eToRemove.Add(e);
                        kToRemove.Add(k);
                    }
                }
            }
            foreach (Konk k in kToRemove)
            {
                konks.Remove(k);
            }
            foreach (Enemy e in eToRemove)
            {
                enemies.Remove(e);
            }
            kToRemove.Clear();
            eToRemove.Clear();

            //check for loss condition
            if (konks.Count < 5)
            {
                Loser = true;
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch sba, SpriteFont sf)
        {
            _spriteBatch = sba;
            spriteFont = sf;

            StringBuilder sb = new StringBuilder();
            sb.Append("Score: ");
            sb.Append(unmodifiedScore.ToString());
            sb.Append($" * Accuracy: {Math.Round(accuracy, 2)} = {(int)score}");

            StringBuilder sb2 = new StringBuilder();
            sb2.Append("High Score: ");
            sb2.Append(parent.HighScore.ToString());
            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (GenericSprite gs in sprites)
            {
                gs.Draw(gameTime, _spriteBatch);
            }

            if (!Loser)
            {
                _spriteBatch.DrawString(spriteFont, "Protect your precious Konks!", new Vector2(0, 0), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            else
            {
                _spriteBatch.DrawString(spriteFont, "YOU HAVE FAILED, press esc to exit to the menu", new Vector2(0, 0), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            _spriteBatch.DrawString(spriteFont, sb.ToString(), new Vector2(5, 850), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            _spriteBatch.DrawString(spriteFont, sb2.ToString(), new Vector2(1000, 850), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            
            foreach (Bullet b in bullets)
            {
                b.Draw(gameTime, _spriteBatch);
            }
            foreach (Enemy e in enemies)
            {
                e.Draw(gameTime, _spriteBatch);
            }
            foreach (Konk k in konks)
            {
                k.Draw(gameTime, _spriteBatch, camera);
            }
            player.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

        }
    }
}
