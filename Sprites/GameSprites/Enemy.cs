using Gonk_Konk_Complete_Edition.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gonk_Konk_Complete_Edition.Sprites.GameSprites
{
    public class Enemy
    {
        Texture2D texture;

        Vector2 Position;
        public Vector2 Velocity;
        bool goingRight;
        int lane;
        bool flipped;
        float scale = 0.25f;

        public BoundingRectangle CollisionBox;

        public Enemy(ContentManager contentManager)
        {
            LoadContent(contentManager);

            //choose a lane and direction
            Random random = new Random(DateTime.Now.Millisecond);
            lane = random.Next(0, 5);
            int t = random.Next(0, 2);
            if (t == 1) goingRight = true;
            else goingRight = false;

            //Put in position and set moving
            if (goingRight) //is on left
            {
                flipped = false;
                Position = new Vector2(-240 * scale, Constants.GAME_HEIGHT - (Constants.GAME_HEIGHT / 5) - ((Constants.GAME_HEIGHT / 5) * lane) + 30);
                Velocity = new Vector2(100, 0);
            }
            else //is on right
            {
                flipped = true;
                Position = new Vector2(Constants.GAME_WIDTH, Constants.GAME_HEIGHT - (Constants.GAME_HEIGHT / 5) - ((Constants.GAME_HEIGHT / 5) * lane) + 30);
                Velocity = new Vector2(-100, 0);
            }
            CollisionBox = new BoundingRectangle(Position, 480 * scale, 240 * scale);
        }
        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("GameContent/StormTrooper");
        }
        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * t;
            CollisionBox.X = Position.X;
            CollisionBox.Y = Position.Y;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionBox.CollidesWith(other);
        }
    }
}
