using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Gonk_Konk_Complete_Edition.Collisions;
namespace Gonk_Konk_Complete_Edition.Sprites.GameSprites
{
    public class Bullet
    {
        Texture2D texture;

        Vector2 Position;
        public Vector2 Velocity;

        public BoundingRectangle CollisionBox;

        float scale = 0.17f;



        public Bullet(ContentManager c, Vector2 p, bool fR)
        {
            LoadContent(c);
            Position = p;
            CollisionBox = new BoundingRectangle(Position, 80 * scale, (400 - 200) * scale);
            if (fR)
            {
                Velocity = new Vector2(1000, 0);
            }
            else
            {
                Velocity = new Vector2(-1000, 0);
            }
        }
        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("GameContent/Laser");
        }
        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * t;
            CollisionBox.X = Position.X + (100 * scale);
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
