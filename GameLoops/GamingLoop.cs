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

namespace Gonk_Konk_Complete_Edition.GameLoops
{
    public class GamingLoop
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont spriteFont;

        private Player player;
        private bool Loser = false;
        private bool playingLoseMusic = false;

        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();
        List<Konk> konks = new List<Konk>();
        int score = 0;

        KeyboardState keyboardCurrent;
        KeyboardState keyboardPrior = Keyboard.GetState();

        System.Random random;
        float spawnTimer = 0.0f;

        private SoundEffect enemyDeath1;
        private SoundEffect enemyDeath2;
        private SoundEffect enemyDeath3;
        private SoundEffect pew;
        private Song backgroundMusic;


        public GamingLoop()
        {
            
           
        }

        public void Initialize(ContentManager content)
        {
            random = new System.Random(DateTime.Now.Millisecond);

            //declare the sprite classes
            player = new Player();
            konks.Add(new Konk(content, 0));
            konks.Add(new Konk(content, 1));
            konks.Add(new Konk(content, 2));
            konks.Add(new Konk(content, 3));
            konks.Add(new Konk(content, 4));
        }

        public void LoadContent(ContentManager content)
        {

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
        }

        public void Update(GameTime gameTime, ContentManager content)
        {
            if (!Loser)
            {
                player.Update(gameTime);

                //handle input for gun
                keyboardCurrent = Keyboard.GetState();
                if (keyboardCurrent.IsKeyDown(Keys.Space) && !(keyboardPrior.IsKeyDown(Keys.Space)))
                {
                    if (player.facingRight)
                    {
                        bullets.Add(new Bullet(content, player.Position + new Vector2(90, -15), player.facingRight));
                    }
                    else
                    {
                        bullets.Add(new Bullet(content, player.Position + new Vector2(-155, -15), player.facingRight));
                    }
                    pew.Play(0.7f, 0, 0);
                }
                keyboardPrior = keyboardCurrent;
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
                spawnTimer = (float)random.Next(3, 11) / 10;
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
                        score += 100;
                    }
                }
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
            if (konks.Count < 4)
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
            sb.Append(score.ToString());
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (!Loser)
            {
                _spriteBatch.DrawString(spriteFont, "Protect at least 4 of your precious Konks!", new Vector2(0, 0), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                _spriteBatch.DrawString(spriteFont, sb.ToString(), new Vector2(5, 850), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            else
            {
                _spriteBatch.DrawString(spriteFont, "YOU HAVE FAILED, press esc to exit", new Vector2(0, 0), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                _spriteBatch.DrawString(spriteFont, sb.ToString(), new Vector2(5, 850), Color.Red, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

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
                k.Draw(gameTime, _spriteBatch);
            }
            player.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

        }
    }
}
