using Gonk_Konk_Complete_Edition.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gonk_Konk_Complete_Edition.Sprites.GameSprites
{
    public class Konk
    {
        Texture2D texture;

        Vector2 Position;

        public BoundingRectangle CollisionBox;

        float scale = 0.5f;

        public Konk3D child;

        public Konk(ContentManager c, int lane, Konk3D childs)
        {
            child = childs;
            LoadContent(c);
            Position = new Vector2(Constants.GAME_WIDTH / 2 - 75, Constants.GAME_HEIGHT - (Constants.GAME_HEIGHT / 5) - ((Constants.GAME_HEIGHT / 5) * lane));
            CollisionBox = new BoundingRectangle(Position, 408 * scale, 256 * scale);
        }
        public void LoadContent(ContentManager contentManager)
        {
            texture = contentManager.Load<Texture2D>("GameContent/Konk");
        }
        public void Update(GameTime gameTime)
        {
            CollisionBox.X = Position.X;
            CollisionBox.Y = Position.Y;
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, CirclingCamera camera)
        {
            //spriteBatch.Draw(texture, Position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            child.Draw(camera);
        }
    }
}
