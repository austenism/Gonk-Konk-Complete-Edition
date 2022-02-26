using Gonk_Konk_Complete_Edition.Sprites.MenuSprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Gonk_Konk_Complete_Edition.GameLoops
{
    public class MenuLoop
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont spriteFont;

        private Gonk mainGonk;
        private Gonk[] smallerGonks;
        private GenericSprite[] sprites;

        private Song backgroundMusic;

        private Random random;
        /// <summary>
        /// contructs the game
        /// </summary>
        public MenuLoop()
        {

        }

        public void Initialize()
        {

            random = new Random(DateTime.Now.Second * DateTime.Now.Minute);


            mainGonk = new Gonk(RotationSpeed.None) { Position = new Vector2(650, 250) };
            smallerGonks = new Gonk[]{
                new Gonk((RotationSpeed)random.Next(1, 6), (float)0.25, true) { Position = new Vector2(80, 400)},
                new Gonk((RotationSpeed)random.Next(1, 6), (float)0.25, true) { Position = new Vector2(190, 400)},
                new Gonk((RotationSpeed)random.Next(1, 6), (float)0.25, true) { Position = new Vector2(300, 400)},
                new Gonk((RotationSpeed)random.Next(1, 6), (float)0.25, true) { Position = new Vector2(410, 400)}
            };

            sprites = new GenericSprite[50];

            sprites[0] = new GenericSprite("Konk", new Vector2(500, 290), 256, 408, (float)0.8, false);
            int ran;
            for (int i = 1; i < 50; i++)
            {
                ran = random.Next(1, 3);
                switch (ran)
                {
                    case 1:
                        sprites[i] = new GenericSprite("star1", new Vector2(random.Next(0, 800), random.Next(0, 480)), 3, 3);
                        break;
                    case 2:
                        sprites[i] = new GenericSprite("star2", new Vector2(random.Next(0, 800), random.Next(0, 480)), 3, 3);
                        break;
                    case 3:
                        sprites[i] = new GenericSprite("star3", new Vector2(random.Next(0, 800), random.Next(0, 480)), 3, 3);
                        break;
                }
            }

        }

        public void LoadContent(ContentManager content)
        {
            mainGonk.LoadContent(content);
            foreach (Gonk gonk in smallerGonks)
            {
                gonk.LoadContent(content);
            }
            foreach (GenericSprite s in sprites)
            {
                s.LoadContent(content);
            }

            backgroundMusic = content.Load<Song>("MenuContent/CantinaBand");
            MediaPlayer.Volume = 0.40f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        public  void Update(GameTime gameTime)
        {
            
        }

        public void Draw(GameTime gameTime, SpriteBatch sb, SpriteFont sf)
        {
            _spriteBatch = sb;
            spriteFont = sf;
            _spriteBatch.Begin();
            //sprites layer 0
            foreach (GenericSprite gs in sprites)
            {
                gs.Draw(gameTime, _spriteBatch);
            }
            foreach (Gonk gonk in smallerGonks)
            {
                gonk.Draw(gameTime, _spriteBatch);
            }

            //words
            _spriteBatch.DrawString(spriteFont, "GONK KONK", new Vector2(65, 90), Color.Blue);
            _spriteBatch.DrawString(spriteFont, "press ESC or B button to exit", new Vector2(0, 0), Color.Blue, 0, new Vector2(0, 0), (float)0.25, SpriteEffects.None, 0);
            _spriteBatch.DrawString(spriteFont, "start", new Vector2(65, 200), Color.Blue, 0, new Vector2(0, 0), (float)0.5, SpriteEffects.None, 0);

            //sprites layer 1
            mainGonk.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
        }
    } 
}
