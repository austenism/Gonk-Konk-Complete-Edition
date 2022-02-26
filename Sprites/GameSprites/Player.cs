using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Gonk_Konk_Complete_Edition.Sprites.GameSprites
{
    public class Player
    {
        private Texture2D texture;
        private Texture2D gunTexture;
        private int width = 256;
        private int height = 408;
        private Vector2 origin;
        private Vector2 gunOrigin;

        private double animationTimer;
        private short animationFrame = 0;
        private bool walking = false;

        private float maxVelocity = 800;

        private Vector2 Velocity = new Vector2(0, 0);


        public static float Scale
        {
            get;
            private set;
        } = (float)0.4;

        public Vector2 Position;
        public bool facingRight = false;

        /// <summary>
        /// constructor for player
        /// </summary>
        public Player()
        {
            origin = new Vector2(width / 2, height / 2);
            gunOrigin = new Vector2(304, 176);
            Position = new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT / 2);
        }

        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("GameContent/Gonk");
            gunTexture = contentManager.Load<Texture2D>("GameContent/Gun");
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            //walking animation
            walking = false;
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.S)) walking = true;

            //turning left and right
            if (facingRight && (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))) facingRight = false;
            if (!facingRight && (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))) facingRight = true;

            //moving

            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if ((keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up)) && (Position.Y - ((height * Scale) / 2)) > Constants.TOP)
            {
                if (Velocity.Y > -maxVelocity) Velocity += new Vector2(0, -25);
            }
            else if ((keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) && (Position.Y + ((height * Scale) / 2)) < Constants.BOTTOM)
            {
                if (Velocity.Y < maxVelocity) Velocity += new Vector2(0, 25);
            }
            else
            {
                Velocity = new Vector2(0, 0);
            }

            Position += Velocity * t;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //update frame
            if (walking) //have the walking frames for this
            {
                if (animationTimer > 0.3)
                {
                    animationFrame++;
                    if (animationFrame > 3) animationFrame = 0;
                    animationTimer -= 0.3;
                }
            }
            else //do not use walking frames use regular frames
            {
                if (animationTimer > 0.6)
                {
                    animationFrame++;
                    if (animationFrame > 1) animationFrame = 0;
                    animationTimer -= 0.6;
                }
            }
            Rectangle source;
            //update the sprite
            if (walking)
            {
                source = new Rectangle(animationFrame * 256 + 1, 0, 256 - 2, 408);
            }
            else
            { //did some math magic here to use every other frame in the animation
                source = new Rectangle((animationFrame * 2 % 4) * 256 + 1, 0, 256 - 2, 408);
            }

            SpriteEffects flipped = SpriteEffects.None;
            if (facingRight) flipped = SpriteEffects.FlipHorizontally;
            //draw gonk
            spriteBatch.Draw(texture, Position, source, Color.White, 0, origin, Scale, flipped, 0);
            //draw gun yes i know i did this a bad way didnt want to redo the sprite
            Vector2 gunPosition;
            if (!facingRight)
            {
                gunPosition = new Vector2(Position.X - 20, Position.Y + 35);
                spriteBatch.Draw(gunTexture, gunPosition, null, Color.White, 0, gunOrigin, Scale * (float)0.75, SpriteEffects.None, 0);

            }
            else
            {
                gunPosition = new Vector2(Position.X + 110, Position.Y + 35);
                spriteBatch.Draw(gunTexture, gunPosition, null, Color.White, 0, gunOrigin, Scale * (float)0.75, SpriteEffects.FlipHorizontally, 0);
            }
        }
    }
}
