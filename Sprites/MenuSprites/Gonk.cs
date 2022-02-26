using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;


namespace Gonk_Konk_Complete_Edition.Sprites.MenuSprites
{
    public enum RotationSpeed
    {
        None = 0,
        Slow = 1,
        Medium = 2,
        Fast = 3,
        Faster = 4,
        Fastest = 5,
        Fasterest = 6
    }
    public class Gonk
    {
        private Texture2D texture;
        private double animationTimer;
        private double spinTimer;
        private short animationFrame = 0;
        private float spinPosition = 0;
        /// <summary>
        /// position of the gonk
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// how fast the gonk rotates
        /// </summary>
        public RotationSpeed RotationSpeed;
        /// <summary>
        /// scaled size of gonk
        /// </summary>
        public float Scale = 1;
        /// <summary>
        /// bool stating whether gonk is flipped
        /// </summary>
        public bool Flipped = false;




        public Gonk(RotationSpeed speed)
        {
            RotationSpeed = speed;
        }
        public Gonk(RotationSpeed speed, float scale, bool flipped)
        {
            RotationSpeed = speed;
            Scale = scale;
            Flipped = flipped;
            spinPosition = MathHelper.Pi * 2;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("MenuContent/Gonk");
        }

        public void Update(GameTime gameTime)
        {

        }
        /// <summary>
        /// draws the gonk
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            spinTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //update animation frame
            if (animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= 0.3;
            }
            //rotate the gonk
            //changes direction based on which way hes facing
            if (!Flipped)
            {
                if (spinTimer > 0.01)
                {
                    spinPosition += (float)0.01 * (float)RotationSpeed;
                    if (spinPosition > (MathHelper.Pi * 2)) spinPosition = 0;
                    spinTimer -= 0.01;
                }
            }
            else
            {
                if (spinTimer > 0.01)
                {
                    spinPosition -= (float)0.01 * (float)RotationSpeed;
                    if (spinPosition < 0) spinPosition = MathHelper.Pi * 2;
                    spinTimer -= 0.01;
                }
            }

            //lets update the sprite yay
            var source = new Rectangle(animationFrame * 256 + 1, 0, 256 - 2, 408);
            SpriteEffects flipped = SpriteEffects.None;
            if (Flipped) flipped = SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, Position, source, Color.White, spinPosition, new Vector2(128, 204), Scale, flipped, 0);
        }
    }
}
